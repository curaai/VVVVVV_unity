using System;
using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class Terminal : MonoBehaviour
    {
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
            if (OnPlayerNow && Input.GetKeyDown(KeyCode.Return))
            {
                var logActivated = log.activeSelf;
                if (logActivated)
                {
                    log.SetActive(false);
                }
                else
                {
                    log.SetActive(true);
                    GetComponent<SpriteGlowEffect>().GlowOn = true;
                    OnceActivated = true;
                    hoverTextBox.SetActive(false);
                }
            }
        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player") && !OnceActivated)
            {
                OnPlayerNow = true;
                hoverTextBox.SetActive(true);
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
