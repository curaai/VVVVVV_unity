using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VVVVVV.UI.Utils.Glow;
using VVVVVV.World.Entity;

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

        public static bool WarpNow = false;

        public override Type controlType => Type.Player;
        public State state { get; private set; }

        private int spriteIdx = 0;
        private int frameDelay = 1;

        void Awake()
        {
            // TODO: Replace to Show Teleport Map
            // OnAction += showTeleportMap;
            OnAction += Teleport;

            state = State.OFF;
        }

        void OnEnable()
        {
            hoverText.SetActive(true);
            SavedTextBoxFrame.SetActive(true); //Disappear automatically
            GetComponentInChildren<SpriteGlowEffect>().GlowOn = true;
            Game.Instance.Save();
        }

        void OnDisable()
        {
            state = State.OFF;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;
            if (state == State.OFF)
            {
                state = State.IDLE;
                enabled = true;
            }
            FocusNow = true;
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            FocusNow = false;
            hoverText.SetActive(false);
        }

        private void showTeleportMap()
        {
            var teleportTimeline = Game.Instance.teleportTimeline;
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

        // public void Teleport(Vector2Int dstPos)
        public void Teleport()
        {
            var player = GameObject.Find("Player").GetComponent<Player>();

            var teleportPos = new Vector2Int(2, 11);
            var newTeleporter = GameObject.Find("World")
                .GetComponentsInChildren<Room>(true)
                .Where(r => r.pos == teleportPos)
                .Select(r => r.GetComponentInChildren<Teleporter>())
                .First();

            // TODO: Append sounds
            // TODO: Refactoring
            IEnumerator Warp()
            {
                WarpNow = true;

                state = State.ON;
                yield return new WaitForSeconds(0.5f);

                // TODO: Change color of teleporter & player
                state = State.OFF;
                // hide player
                player.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);

                player.gameObject.SetActive(true);
                newTeleporter.state = State.ON;

                Game.Instance.ChangeRoom(teleportPos, newTeleporter.transform.localPosition);

                var moveVelocityQueue = newTeleporter.GetComponent<MoveVelocityQueue>();
                if (moveVelocityQueue != null)
                {
                    moveVelocityQueue.enabled = true;
                    moveVelocityQueue.TargetController = player.GetComponent<MoveController>();
                }

                WarpNow = false;
                yield return null;
            }

            StartCoroutine(Warp());


            Debug.Log(newTeleporter);
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