using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World
{
    public class Teleporter : IControllable
    {
        [SerializeField] private GameObject teleportTab;
        [SerializeField] private GameObject SavedTextBoxFrame;
        [SerializeField] private GameObject hoverText;
        [SerializeField] private Sprite[] sprites;

        public override Type controlType => Type.Player;

        private int spriteIdx = 0;
        private int frameDelay = 1;

        void Awake()
        {
            OnAction += ShowTeleportMap;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;
            enabled = true;

            FocusNow = true;
            hoverText.SetActive(true);
            SavedTextBoxFrame.SetActive(true); //Disappear automatically
            GameObject.Find("World").GetComponent<Game>().Save();
            GetComponentInChildren<SpriteGlowEffect>().GlowOn = true;
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            enabled = false;
            FocusNow = false;
            hoverText.SetActive(false);
        }

        void ShowTeleportMap()
        {
            var panelCtrl = GameObject.Find("PanelController").GetComponent<UI.PanelController>();
            panelCtrl.Toggle(teleportTab);
        }

        void Update()
        {
            void UpdateSprite()
            {
                void SetSprite(int idx)
                {
                    GetComponentInChildren<SpriteRenderer>().sprite = sprites[idx];
                }

                frameDelay--;
                if (frameDelay <= 0)
                {
                    frameDelay = 3;
                    var newframe = Mathf.FloorToInt(Utils.RandomHelper.fRand() * 6);
                    if (4 <= newframe)
                    {
                        frameDelay = 12;
                        SetSprite(1);
                    }
                    else
                    {
                        SetSprite(1 + newframe);
                    }
                }
            }

            UpdateSprite();
        }
    }
}