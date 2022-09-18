using System;
using UnityEngine;
using VVVVVV.Utils;

namespace VVVVVV.UI
{
    public class CutSceneController : MonoBehaviour
    {
        public (GameObject, Transform)? mainUI;

        private Animator slideAnimator => GetComponent<Animator>();

        public void Open(GameObject uiObj)
        {
            var container = transform.GetChild(1);
            uiObj.transform.SetParent(container);
            uiObj.SetActive(true);
            // save original parent;
            mainUI = (uiObj, uiObj.transform.parent);

            slideAnimator.SetBool("Open", true);

            var player = GameObject.Find("Player");
            player.GetComponent<World.Entity.MoveController>().Stop();
            player.GetComponent<PlayerInputManager>().enabled = false;
        }

        void Update()
        {
            if (mainUI?.Item1.activeSelf ?? false && Input.GetKeyDown(KeyCode.Space))
            {
                // TODO: Only Debug 
                return;

                slideAnimator.SetBool("Open", false);
                StartCoroutine(AnimationHelper.CheckAnimationCompleted(
                    slideAnimator, "Close", () => Close()
                ));
            }
        }

        public void Close()
        {
            if (mainUI.HasValue)
            {
                var obj = mainUI.Value.Item1;
                obj.SetActive(false);
                obj.transform.SetParent(mainUI.Value.Item2);
                mainUI = null;
            }
        }
    }
}
