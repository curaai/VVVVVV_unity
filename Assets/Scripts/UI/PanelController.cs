using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VVVVVV.UI
{
    public class PanelController : Utils.Singleton<PanelController>
    {
        private readonly string ANIMATOR_TRANSITION_KEY = "open";

        private Animator animator;

        private (GameObject, Transform)? curUi = null;
        public bool OpenedNow => curUi.HasValue;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Open(GameObject ui)
        {
            if (isPlayingNow()) return;

            if (!curUi.HasValue)
            {
                Debug.Log($"[Panel] Open {ui.name}");

                animator.SetBool(ANIMATOR_TRANSITION_KEY, true);
                activateCurUi(ui);

                StartCoroutine(Utils.AnimatorHelper.SetAnimStateCallback(
                    animator,
                    "UP",
                    () => InputManager.Instance.Focus(ui.GetComponent<IInputtable>())
                ));
            }
            else if (curUi?.Item1 != ui)
            {
                Debug.Log($"[Panel] Replaced to {ui.name}");

                deactiveCurUi();
                activateCurUi(ui);
            }
        }

        public void Close(GameObject ui)
        {
            if (isPlayingNow()) return;

            if (curUi?.Item1 == ui)
            {
                Debug.Log($"[Panel] Close {curUi.Value.Item1.name}");

                animator.SetBool(ANIMATOR_TRANSITION_KEY, false);

                StartCoroutine(Utils.AnimatorHelper.SetAnimStateCallback(
                    animator,
                    "DOWN",
                    () =>
                    {
                        deactiveCurUi();
                        InputManager.Instance.Defocus();
                    }
                ));
            }
        }

        void activateCurUi(GameObject ui)
        {
            curUi = (ui, ui.transform);
            ui.transform.SetParent(transform);
            ui.SetActive(true);
        }

        void deactiveCurUi()
        {
            var obj = curUi.Value.Item1;
            var ts = curUi.Value.Item2;

            obj.SetActive(false);
            obj.transform.SetParent(ts);
            curUi = null;
        }

        bool isPlayingNow()
        {
            var ANIMATOR_IDLE_STATE_NAME = "IDLE";
            var state = animator.GetCurrentAnimatorStateInfo(0);
            var isIdleState = state.IsName(ANIMATOR_IDLE_STATE_NAME);

            return !isIdleState && state.normalizedTime < 1;
        }
    }
}
