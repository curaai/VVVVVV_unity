using UnityEngine;
using VVVVVV.UI.Utils;

namespace VVVVVV.UI
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private Transform archiveObj;

        public SlidePanel mainUI { get; private set; }
        private Animator slideAnimator => GetComponent<Animator>();

        public void Open()
        {
            if (!SlidePanel.Opened)
            {
                mainUI.transform.SetParent(transform);
                mainUI.Open();
            }
        }
        public void Close()
        {
            if (SlidePanel.Opened)
            {
                mainUI.transform.SetParent(archiveObj);
                mainUI.Close();
            }
            mainUI = null;
        }

        public void Toggle()
        {
            slideAnimator.SetBool("open", !SlidePanel.Opened);
        }

        public void SetMainUI(SlidePanel ui)
        {
            mainUI = ui;
        }
    }
}