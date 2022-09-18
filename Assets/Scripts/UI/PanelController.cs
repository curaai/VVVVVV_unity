using UnityEngine;
using VVVVVV.UI.Utils;

namespace VVVVVV.UI
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private Transform archiveObj;

        public (GameObject, Transform)? mainUI;
        private Animator slideAnimator => GetComponent<Animator>();
        public bool Opened => GetComponent<RectTransform>().anchoredPosition.y == 480;

        private void Open(GameObject obj)
        {
            mainUI = (obj, obj.transform.parent);
            if (!Opened)
                slideAnimator.SetBool("open", true);

            if (mainUI.HasValue)
            {
                var container = transform.GetChild(1);
                mainUI?.Item1.transform.SetParent(container);
                mainUI?.Item1.SetActive(true);
            }
        }
        private void Close()
        {
            if (Opened)
            {
                slideAnimator.SetBool("open", false);
                VVVVVV.Utils.AnimationHelper.CheckAnimationCompleted(slideAnimator, "close", () =>
                {
                    mainUI?.Item1.transform.SetParent(mainUI?.Item2);
                    mainUI?.Item1.SetActive(false);
                    mainUI = null;
                });
            }
        }

        public void Toggle(GameObject ui)
        {
            if (mainUI?.Item1 == ui)
                Close();
            else
                Open(ui);
        }
    }
}
