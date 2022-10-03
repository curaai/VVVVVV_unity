using System;
using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class Terminal : IControllable
    {
        [SerializeField] private AudioClip terminalSound;
        [SerializeField] private AudioClip terminalOpenSound;
        [SerializeField] private GameObject hoverTextBox;
        [SerializeField] private GameObject log;

        public override Type controlType => IControllable.Type.UI;

        public bool OnPlayerNow { get; private set; }
        public bool OnceActivated { get; private set; }

        void Awake()
        {
            OnceActivated = false;
            OnAction += OpenLog;
        }

        void OpenLog()
        {
            if (!OnceActivated)
            {
                OnceActivated = true;

                GetComponentInChildren<SpriteGlowEffect>().GlowOn = true;
                hoverTextBox.SetActive(false);
                SoundManager.Instance.PlayEffect(terminalOpenSound);

                GameObject.Find("CutScene").GetComponent<UI.CutSceneController>().Open(log);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player") && !OnceActivated)
            {
                FocusNow = true;
                OnPlayerNow = true;
                hoverTextBox.SetActive(true);
                SoundManager.Instance.PlayEffect(terminalSound);
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                FocusNow = false;
                OnPlayerNow = false;
                hoverTextBox.SetActive(false);
            }
        }
    }
}
