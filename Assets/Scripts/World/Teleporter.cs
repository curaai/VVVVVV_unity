using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World
{
    public class Teleporter : IControllable
    {
        public enum State
        {
            OFF,
            IDLE,
            ON
        }

        [SerializeField] private GameObject teleportTab;
        [SerializeField] private GameObject SavedTextBoxFrame;
        [SerializeField] private GameObject hoverText;
        [SerializeField] private Sprite[] sprites;

        public override Type controlType => Type.Player;
        public State state { get; private set; }

        private int spriteIdx = 0;
        private int frameDelay = 1;

        void Awake()
        {
            OnAction += showTeleportMap;
            state = State.OFF;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;
            state = State.IDLE;

            enabled = true;
            FocusNow = true;
            hoverText.SetActive(true);
            SavedTextBoxFrame.SetActive(true); //Disappear automatically
            GetComponentInChildren<SpriteGlowEffect>().GlowOn = true;
            GameObject.Find("World").GetComponent<Game>().Save();
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            FocusNow = false;
            hoverText.SetActive(false);
        }

        private void showTeleportMap()
        {
            var teleportTimeline = GameObject.Find("Game").GetComponent<Game>().teleportTimeline;
            if (teleportTimeline != null)
            {
                teleportTimeline.Play();
            }
            else
            {
                var panelCtrl = GameObject.Find("PanelController").GetComponent<UI.PanelController>();
                panelCtrl.Toggle(teleportTab);
            }
        }

        public void Teleport(Vector2Int dstPos)
        {
            state = State.ON;
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
                    if (state == State.IDLE)
                        frameDelay = 3;
                    else if (state == State.ON)
                        frameDelay = 6;

                    var newframe = Mathf.FloorToInt(Utils.RandomHelper.fRand() * 6);
                    if (4 <= newframe)
                    {
                        frameDelay = 12;
                        SetSprite(1);
                    }
                    else
                    {
                        if (state == State.IDLE)
                            SetSprite(1 + newframe);
                        else if (state == State.ON)
                            SetSprite(5 + newframe);
                    }
                }
            }

            if (state == State.OFF) return;

            UpdateSprite();
        }
    }
}