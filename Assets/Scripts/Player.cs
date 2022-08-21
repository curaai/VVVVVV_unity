using System;
using UnityEngine;
using VVVVVV.World.Entity;


namespace VVVVVV
{
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(MoveController))]
    public class Player : MonoBehaviour
    {
        const string DAMAGABLE_TAG = "Damagable";

        Animator animator;
        MoveController controller;
        UI.Minimap minimap;
        Game game;

        Room room;

        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<MoveController>();
            game = GameObject.Find("Game").GetComponent<Game>();
            minimap = game.minimap;

            room = minimap.room(new Vector2Int(115, 103));
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

            void ChangeRoom(Vector2Int rpos)
            {
                room = minimap.room(rpos);

                var game = GameObject.Find("Game").GetComponent<Game>();
                game.ChangeRoom(room.pos);

                transform.SetParent(minimap.roomObj(room).transform);
            }

            var newRoomDir = IsRoomChanged();
            if (newRoomDir.HasValue)
            {
                ChangeRoom(room.pos + newRoomDir.Value);
            }
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
    }
}