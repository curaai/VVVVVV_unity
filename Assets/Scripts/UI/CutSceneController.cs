using System;
using UnityEngine;
using VVVVVV.Utils;

namespace VVVVVV.UI
{
    public class CutSceneController : IControllable
    {
        public override Type controlType => Type.UI;

        public (GameObject, Transform)? mainUI;
        private Animator slideAnimator => GetComponent<Animator>();
        void Awake()
        {
            OnSpace += Close;
            OnMove += DummyMoveFunc;
        }

        public void Open(GameObject uiObj)
        {
            FocusNow = true;

            var container = transform.GetChild(1);
            uiObj.transform.SetParent(container);
            uiObj.SetActive(true);

            // save original parent;
            mainUI = (uiObj, uiObj.transform.parent);

            slideAnimator.SetBool("Open", true);

            var player = GameObject.Find("Player");
            player.GetComponent<World.Entity.MoveController>().Stop();
            player.GetComponent<PlayerInputManager>().enabled = false;

            Debug.Log($"Open Cutscene {uiObj.name}");
        }

        public void Close()
        {
            slideAnimator.SetBool("Open", false);
            if (mainUI.HasValue)
            {
                StartCoroutine(AnimationHelper.CheckAnimationCompleted(
                    slideAnimator, "Close", () =>
                    {
                        FocusNow = false;

                        var obj = mainUI.Value.Item1;
                        obj.SetActive(false);
                        obj.transform.SetParent(mainUI.Value.Item2);
                        mainUI = null;

                        var player = GameObject.Find("Player");
                        player.GetComponent<PlayerInputManager>().enabled = true;

                        Debug.Log("Close Cutscene");
                    }
                ));
            }
        }
    }
}
