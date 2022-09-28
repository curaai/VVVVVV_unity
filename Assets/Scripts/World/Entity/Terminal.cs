using System;
using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private AudioClip terminalSound;
        [SerializeField] private AudioClip terminalOpenSound;
        [SerializeField] private GameObject hoverTextBox;
        [SerializeField] private GameObject log;

        public bool OnPlayerNow { get; private set; }
        public bool OnceActivated { get; private set; }

        void OnEabled()
        {
            OnceActivated = false;
        }

        void Update()
        {
            if (!log.activeSelf && !OnceActivated && OnPlayerNow && Input.GetKeyDown(KeyCode.Return))
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
                OnPlayerNow = true;
                hoverTextBox.SetActive(true);
                SoundManager.Instance.PlayEffect(terminalSound);
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                OnPlayerNow = false;
                hoverTextBox.SetActive(false);
            }
        }
    }
}
