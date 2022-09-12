using System;
using UnityEngine;
using VVVVVV.Utils;

namespace VVVVVV.UI
{
    public class CutSceneController : MonoBehaviour
    {
        private Transform originalParent;
        private Action closeCallback;

        public GameObject mainUI;
        private Animator slideAnimator => GetComponent<Animator>();

        public void Open(GameObject uiObj)
        {
            originalParent = uiObj.transform.parent;
            // this.closeCallback = closeCallback;

            mainUI = uiObj;
            mainUI.transform.SetParent(transform);
            mainUI.SetActive(true);
            slideAnimator.SetBool("Open", true);
        }

        void Update()
        {
            if (mainUI != null && mainUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
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
            mainUI.transform.SetParent(originalParent);
            mainUI.SetActive(false);
            originalParent = null;
            mainUI = null;

            if (closeCallback != null)
                closeCallback();
        }
    }
}
