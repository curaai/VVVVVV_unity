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

        public void Toggle(SlidePanel ui)
        {
            // close 
            if (mainUI == ui)
            {
                slideAnimator.SetBool("open", false);
            }
            // replace
            else if (mainUI != null && mainUI != ui)
            {
                Close();
                mainUI = ui;
                Open();
            }
            // new Open
            else
            {
                mainUI = ui;
                slideAnimator.SetBool("open", true);
            }
        }
    }
}
