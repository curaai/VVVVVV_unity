using UnityEngine;

namespace VVVVVV.UI
{
    public class SlidePanel : MonoBehaviour
    {
        [SerializeField] protected GameObject uiPanel;
        [SerializeField] protected GameObject mainPanel;

        private PlayerMove player => GameObject.Find("Player").GetComponent<PlayerMove>();

        public static bool Opened = false;

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var controller = mainPanel.GetComponent<PanelController>();
                if (!Opened)
                {
                    controller.SetMainUI(this);
                }
                controller.Toggle();
            }
        }

        // Get Event from Animator
        internal virtual void Open()
        {
            activeChildren(true);
            uiPanel.transform.SetParent(mainPanel.transform);
            Opened = true;

            player.enabled = false;
        }

        internal virtual void Close()
        {
            activeChildren(false);
            uiPanel.transform.SetParent(null);

            Opened = false;

            // TODO: refactoring need
            player.enabled = true;
        }

        private void activeChildren(bool activate)
        {
            foreach (Transform child in uiPanel.transform)
                child.gameObject.SetActive(activate);
        }
    }
}