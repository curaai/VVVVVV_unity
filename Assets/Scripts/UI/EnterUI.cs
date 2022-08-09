using UnityEngine;

namespace VVVVVV.UI
{
    public class EnterUI : MonoBehaviour
    {
        [SerializeField] protected GameObject uiPanel;
        [SerializeField] protected GameObject mainPanel;

        private Animator slideAnimator => mainPanel.GetComponent<Animator>();
        private PlayerMove player => GameObject.Find("Player").GetComponent<PlayerMove>();

        public static bool Paused = false;

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                slideAnimator.SetBool("open", !Paused);
            }
        }

        // Get Event from Animator
        protected virtual void Pause()
        {
            uiPanel.SetActive(true);
            uiPanel.transform.SetParent(mainPanel.transform);
            Paused = true;

            player.enabled = false;
        }

        protected virtual void Resume()
        {
            uiPanel.SetActive(false);
            uiPanel.transform.SetParent(null);

            Paused = false;

            // TODO: refactoring need
            player.enabled = true;
        }
    }
}