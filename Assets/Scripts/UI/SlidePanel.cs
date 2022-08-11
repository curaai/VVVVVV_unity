using UnityEngine;

namespace VVVVVV.UI
{
    public class SlidePanel : MonoBehaviour
    {
        [SerializeField] protected GameObject uiPanel;
        [SerializeField] protected GameObject mainPanel;

        private Animator slideAnimator => mainPanel.GetComponent<Animator>();
        private PlayerMove player => GameObject.Find("Player").GetComponent<PlayerMove>();

        public static bool Opened = false;

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }
        }

        // Get Event from Animator
        protected virtual void Open()
        {
            activeChildren(true);
            uiPanel.transform.SetParent(mainPanel.transform);
            Opened = true;

            player.enabled = false;
        }

        protected virtual void Close()
        {
            activeChildren(false);
            uiPanel.transform.SetParent(null);

            Opened = false;

            // TODO: refactoring need
            player.enabled = true;
        }

        public void Toggle()
        {
            slideAnimator.SetBool("open", !Opened);
        }

        private void activeChildren(bool activate)
        {
            foreach (Transform child in uiPanel.transform)
                child.gameObject.SetActive(activate);
        }
    }
}