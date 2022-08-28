using System;
using UnityEngine;
using VVVVVV.World.Entity;


namespace VVVVVV
{
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(MoveController))]
    public class Player : MonoBehaviour, ISerializable
    {
        const string DAMAGABLE_TAG = "Damagable";
        public string SerializeKey => "Player";

        [SerializeField] private Game game;

        private Animator animator;
        private MoveController controller;
        private UI.Minimap minimap;

        public Room room;
        public int deathCount = 0;

        void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<MoveController>();
            minimap = game.minimap;
        }

        void Update()
        {
            Vector2Int? IsRoomChanged()
            {
                var lpos = transform.localPosition;
                var res = Vector2Int.zero;

                if (lpos.x <= 0f) res.x = -1;
                else if (40f < lpos.x) res.x = 1;

                if (lpos.y <= -1.5f) res.y = 1;
                else if (28.25f < lpos.y) res.y = -1;

                return res == Vector2Int.zero ? (Vector2Int?)null : res;
            }

            var newRoomDir = IsRoomChanged();
            if (newRoomDir.HasValue)
            {
                changeRoom(room.pos + newRoomDir.Value);
            }
        }

        private void changeRoom(Vector2Int rpos)
        {
            room = minimap.room(rpos);

            var game = GameObject.Find("Game").GetComponent<Game>();
            game.ChangeRoom(room.pos);

            transform.SetParent(minimap.roomObj(room).transform);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(DAMAGABLE_TAG))
                GameOver();
        }

        public void GameOver()
        {
            animator.SetBool("HurtNow", true);

            GetComponent<Collider2D>().isTrigger = true;
            controller.enabled = false;
            controller.force = Vector2.zero;
            controller.velocity = Vector2.zero;

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StartCoroutine(Utils.AnimationHelper.CheckAnimationCompleted(animator, "Hurt", () =>
            {
                game.Respawn();
                animator.SetBool("HurtNow", false);
            }));
        }

        public string Save()
        {
            var p = Savepoint.LastSavepoint.transform.position;

            return SaveManager.SerializableObject(
                new SaveInfo()
                {
                    direction = controller.direction,
                    gravity = controller.gravity,
                    roomPos = (room.pos.x, room.pos.y),
                    position = (p.x, p.y, p.z),
                }
            );
        }

        public void Load(string str)
        {
            if (str == "")
            {
                // Only Use for Development when save data clear
                changeRoom(new Vector2Int(115, 103));
                return;
            }

            GetComponent<Collider2D>().isTrigger = false;
            controller.enabled = true;

            var x = SaveManager.DeserializeObject<SaveInfo>(str);
            controller.direction = x.direction;
            controller.ReverseGravity(x.gravity);

            changeRoom(new Vector2Int(x.roomPos.Item1, x.roomPos.Item2));
            transform.position = new Vector3(x.position.Item1, x.position.Item2, x.position.Item3);

            deathCount = x.deathCount;
        }


        [Serializable]
        struct SaveInfo
        {
            public Direction direction;
            public Gravity gravity;
            public (int, int) roomPos;
            public (float, float, float) position;
            public int deathCount;
        }
    }
}