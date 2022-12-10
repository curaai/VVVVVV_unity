using System;
using UnityEngine;
using VVVVVV.World.Entity;


namespace VVVVVV
{
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(MoveController))]
    public class Player : MonoBehaviour, ISerializable
    {
        const string DAMAGABLE_TAG = "Damagable";

        [SerializeField] private Game game;
        [SerializeField] private AudioClip deathSound;

        private Animator animator;
        private MoveController controller;

        public bool IsMoveNow { get; private set; }


        public int deathCount = 0;

        void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<MoveController>();
        }

        void FixedUpdate()
        {
            var v = controller.velocity;
            animator.SetBool("MoveNow", v.x != 0);
            animator.SetBool("JumpNow", v.y != 0);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(DAMAGABLE_TAG))
                GameOver();
        }

        public void GameOver()
        {
            SoundManager.Instance.PlayEffect(deathSound);

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

        string ISerializable.Serialize()
        {
            var p = Savepoint.LastSavepoint.transform.position;

            return Utils.SerializeHelper.SerializeObject(
                new SaveInfo()
                {
                    direction = controller.direction,
                    gravity = controller.gravity,
                    position = (p.x, p.y, p.z),
                    deathCount = deathCount,
                }
            );
        }

        void ISerializable.LoadSerializedData(string str)
        {
            GetComponent<Collider2D>().isTrigger = false;

            var x = Utils.SerializeHelper.DeserializeObject<SaveInfo>(str);
            controller.enabled = true;
            controller.direction = x.direction;
            controller.ReverseGravity(x.gravity);

            transform.position = new Vector3(x.position.Item1, x.position.Item2, x.position.Item3);

            deathCount = x.deathCount;
        }

        [Serializable]
        struct SaveInfo
        {
            public Direction direction;
            public Gravity gravity;
            public (float, float, float) position;
            public int deathCount;
        }
    }
}