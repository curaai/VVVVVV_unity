using UnityEngine;
using VVVVVV.UI.Utils;

namespace VVVVVV.UI
{
    public class PanelController : IControllable
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private AudioClip pauseSound;
        [SerializeField] private GameObject mapPanel;

        public (GameObject, Transform)? mainUI;
        private Animator slideAnimator => GetComponent<Animator>();
        public bool Opened => GetComponent<RectTransform>().anchoredPosition.y == 480;
        public override Type controlType => Type.UI;

        void Awake()
        {
            OnAction += ToggleMap;
        }

        void OnEnable() => FocusNow = true;
        void OnDisable() => FocusNow = false;

        private void Open(GameObject obj)
        {
            mainUI = (obj, obj.transform.parent);
            if (!Opened)
                slideAnimator.SetBool("open", true);

            if (mainUI.HasValue)
            {
                mainUI?.Item1.transform.SetParent(transform);
                mainUI?.Item1.SetActive(true);
            }
        }
        private void Close()
        {
            if (Opened)
            {
                slideAnimator.SetBool("open", false);
                StartCoroutine(VVVVVV.Utils.AnimationHelper.CheckAnimationCompleted(slideAnimator, "DOWN", () =>
                {
                    mainUI?.Item1.transform.SetParent(mainUI?.Item2);
                    mainUI?.Item1.SetActive(false);
                    mainUI = null;
                }));
            }
        }

        public void Toggle(GameObject ui)
        {
            if (ui == null || mainUI?.Item1 == ui)
                Close();
            else
                Open(ui);
        }

        public void TogglePause()
        {
            if (mainUI == null || mainUI?.Item1 == pausePanel)
            {
                if (mainUI != null) SoundManager.Instance.PlayEffect(pauseSound);
                Toggle(pausePanel);
            }
        }

        public void ToggleMap()
        {
            if (mainUI == null || mainUI?.Item1 == mapPanel)
            {
                Toggle(mapPanel);
            }
        }
    }
}
