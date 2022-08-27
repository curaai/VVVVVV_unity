using UnityEngine;
using VVVVVV.UI.Utils;

namespace VVVVVV.UI
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private Transform archiveObj;

        public SlidePanel mainUI;
        private Animator slideAnimator => GetComponent<Animator>();

        public void Open()
        {
            if (!mainUI.enabled)
            {
                mainUI.transform.SetParent(transform);
                mainUI.enabled = true;
            }
        }
        public void Close()
        {
            if (mainUI.enabled)
            {
                mainUI.transform.SetParent(archiveObj);
                mainUI.enabled = false;
            }
            mainUI = null;
        }

        public void Toggle()
        {
            slideAnimator.SetBool("open", !mainUI.enabled);
        }

        public void SetMainUI(SlidePanel ui)
        {
            mainUI = ui;
        }
    }
}