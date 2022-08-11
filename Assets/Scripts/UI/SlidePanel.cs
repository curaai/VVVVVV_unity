using UnityEngine;

namespace VVVVVV.UI
{
    public class SlidePanel : MonoBehaviour
    {
        private PlayerMove player => GameObject.Find("Player").GetComponent<PlayerMove>();

        public static bool Opened = false;

        // Get Event from Animator
        internal virtual void Open()
        {
            activeChildren(true);
            Opened = true;

            player.enabled = false;
        }

        internal virtual void Close()
        {
            activeChildren(false);
            Opened = false;

            // TODO: refactoring need
            player.enabled = true;
        }

        public void Toggle()
        {
            var controller = GameObject.Find("RootPanel").GetComponent<PanelController>();
            if (!Opened)
            {
                controller.SetMainUI(this);
            }
            controller.Toggle();
        }

        private void activeChildren(bool activate)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(activate);
        }
    }
}