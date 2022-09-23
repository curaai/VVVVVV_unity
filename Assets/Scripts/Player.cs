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
        }
        void Start()
        {
            minimap = game.minimap;
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
                    position = (p.x, p.y, p.z),
                    deathCount = deathCount,
                }
            );
        }

        public void Load(string str)
        {
            if (str == "") return;

            GetComponent<Collider2D>().isTrigger = false;

            var x = SaveManager.DeserializeObject<SaveInfo>(str);
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