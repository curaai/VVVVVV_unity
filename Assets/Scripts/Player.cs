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
                int xDir = 0;
                if (lpos.x <= 0f)
                    xDir = -1;
                else if (40f < lpos.x)
                    xDir = 1;

                int yDir = 0;
                if (lpos.y <= -1.5f)
                    yDir = 1;
                else if (28.25f < lpos.y)
                    yDir = -1;

                if (xDir == 0 && yDir == 0)
                    return null;
                else
                    return new Vector2Int(xDir, yDir);
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
            {
                animator.SetBool("HurtNow", true);
                GameOver();
            }
        }

        public void GameOver()
        {
            GetComponent<Collider2D>().isTrigger = true;
            controller.force = Vector2.zero;
            controller.velocity = Vector2.zero;
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

            var x = SaveManager.DeserializeObject<SaveInfo>(str);
            controller.direction = x.direction;
            controller.gravity = x.gravity;
            GetComponent<PlayerInputManager>().SetGravityForce();

            changeRoom(new Vector2Int(x.roomPos.Item1, x.roomPos.Item2));
            transform.position = new Vector3(x.position.Item1, x.position.Item2, x.position.Item3);
        }


        [Serializable]
        struct SaveInfo
        {
            public Direction direction;
            public Gravity gravity;
            public (int, int) roomPos;
            public (float, float, float) position;
        }
    }
}