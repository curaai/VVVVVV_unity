using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VVVVVV.UI.Panels
{
    public class PausePanel : MonoBehaviour, IInputtable
    {
        [SerializeField] protected List<Glow.TextFocusGlow> options;

        private int CurOptIdx;

        private void OnEnable()
        {
            SetOptionIndex(0);
        }

        private void OnPause(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (PanelController.Instance.OpenedNow)
                    PanelController.Instance.Close(gameObject);
                else
                    PanelController.Instance.Open(gameObject);
            }
        }

        void IInputtable.OnMoveInput(float v)
        {
            if (v == 0) return;

            var dir = 0 < v ? 1 : -1;
            var idx = CurOptIdx + dir;
            if (idx < 0) idx = options.Count - 1;
            else if (options.Count <= idx) idx = 0;

            SetOptionIndex(idx);
        }
        public void SetOptionIndex(int idx)
        {
            if (idx < 0 || options.Count <= idx) return;

            CurOptIdx = idx;

            for (int i = 0; i < options.Count; i++)
                options[i].GlowOn = i == CurOptIdx;
        }

        void IInputtable.OnSpaceInput() { DoCurrentOption(); }
        public void DoCurrentOption() { return; }

        void IInputtable.OnActionInput() { return; }
    }
}