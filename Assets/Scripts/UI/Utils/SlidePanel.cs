using UnityEngine;
using VVVVVV.World.Entity;

namespace VVVVVV.UI.Utils
{
    public class SlidePanel : MonoBehaviour
    {
        private MoveController player => GameObject.Find("Player").GetComponent<MoveController>();

        protected virtual void OnEnable()
        {
            activeChildren(true);
            player.enabled = false;
        }

        protected virtual void OnDisable()
        {
            activeChildren(false);

            // TODO: refactoring need
            player.enabled = true;
        }

        public void Toggle()
        {
            var controller = GameObject.Find("RootPanel").GetComponent<PanelController>();
            if (!enabled)
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