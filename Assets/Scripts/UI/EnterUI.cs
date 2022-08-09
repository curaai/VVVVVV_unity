using UnityEngine;

namespace VVVVVV.UI
{
    public class EnterUI : MonoBehaviour
    {
        [SerializeField] protected GameObject uiPanel;

        public static bool Paused = false;

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Paused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        protected virtual void Resume()
        {
            uiPanel.SetActive(false);
            // !TODO: Change pause way, timeScale can make side effect
            Time.timeScale = 1f;
            Paused = false;
        }

        protected virtual void Pause()
        {
            uiPanel.SetActive(true);
            Time.timeScale = 0f;
            Paused = true;
        }
    }
}