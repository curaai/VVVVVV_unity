using System;
using UnityEngine;
using VVVVVV.Utils;

namespace VVVVVV.UI
{
    public class CutSceneController : MonoBehaviour
    {
        public bool ControlInput { get; set; } = true;

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

            Debug.Log($"Open Cutscene {uiObj.name}");
        }

        void Update()
        {
            if ((mainUI?.Item1.activeSelf ?? false) &&
                Input.GetKeyDown(KeyCode.Space) &&
                ControlInput)
            {
                Close();
            }
        }

        public void Close()
        {
            slideAnimator.SetBool("Open", false);
            if (mainUI.HasValue)
            {
                StartCoroutine(AnimationHelper.CheckAnimationCompleted(
                    slideAnimator, "Close", () =>
                    {
                        var obj = mainUI.Value.Item1;
                        obj.SetActive(false);
                        obj.transform.SetParent(mainUI.Value.Item2);
                        mainUI = null;

                        var player = GameObject.Find("Player");
                        player.GetComponent<PlayerInputManager>().enabled = true;
                        ControlInput = true;

                        Debug.Log("Close Cutscene");
                    }
                ));
            }
        }
    }
}
