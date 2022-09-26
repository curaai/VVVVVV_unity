using System;
using System.Linq;
using UnityEngine;

namespace VVVVVV.World.Entity
{
    public enum Gravity
    {
        UP,
        DOWN,
    }
    public enum Direction
    {
        LEFT,
        RIGHT,
    }

    [RequireComponent(typeof(EntityCollider))]
    public class MoveController : MonoBehaviour
    {
        public static readonly Vector2 MaxSpeed = new Vector2(12, 20);
        public static readonly Vector2 INERTIA = new Vector2(2.2f, 0.5f);
        private static readonly float VelocityCalibrationParameter = 25;

        [SerializeField] bool UpdateSprite;
        [SerializeField] bool ApplyFriction;

        public Gravity gravity
        {
            get => _gravity;
            set
            {
                _gravity = value;

                if (!UpdateSprite) return;
                var s = transform.localScale;
                transform.localScale = new Vector3(s.x, gravity == Gravity.DOWN ? 1 : -1, s.z);
            }
        }
        private Gravity _gravity;
        public Direction direction
        {
            get => _direction;
            set
            {
                _direction = value;
                if (!UpdateSprite) return;
                var s = transform.localScale;
                transform.localScale = new Vector3(direction == Direction.RIGHT ? 1 : -1, s.y, s.z);
            }
        }
        private Direction _direction;

        public Vector2 force;
        public Vector2 velocity;
        public Vector2 AdditionalVelocity = Vector2.zero;

        private EntityCollider entityCollider;

        public Vector3? StartPos = null;

        void Awake()
        {
            entityCollider = GetComponent<EntityCollider>();
        }

        void OnEnable()
        {
            if (gameObject.CompareTag("Player"))
                return;

            if (StartPos == null)
                StartPos = transform.localPosition;
            else
                transform.localPosition = StartPos.Value;
        }

        void FixedUpdate()
        {
            Vector2 applyFriction(Vector2 v)
            {
                Vector2 sign = new Vector2(0 < v.x ? 1 : -1, 0 < v.y ? 1 : -1);

                (v.x, v.y) = (Mathf.Abs(v.x), Mathf.Abs(v.y));

                v -= INERTIA;
                v = Vector2.Min(v, MaxSpeed);

                if (v.x < INERTIA.x) v.x = 0;
                if (v.y < INERTIA.y) v.y = 0;

                return v * sign;
            }

            if ((entityCollider.OnWallLeft && direction == Direction.RIGHT) ||
                (entityCollider.OnWallRight && direction == Direction.LEFT) ||
                !(entityCollider.OnWallLeft || entityCollider.OnWallRight))
                velocity.x = velocity.x + force.x;

            if ((entityCollider.OnGround && gravity == Gravity.UP) ||
                (entityCollider.OnRoof && gravity == Gravity.DOWN) ||
                !(entityCollider.OnGround || entityCollider.OnRoof))
                velocity.y = velocity.y + force.y;

            if (ApplyFriction) velocity = applyFriction(velocity);
            GetComponent<Rigidbody2D>().velocity = (AdditionalVelocity + velocity) * VelocityCalibrationParameter;
        }

        public void Stop()
        {
            force.x = 0f;
            velocity.x = 0f;
        }

        public void ToGround()
        {
            if (!entityCollider.OnRoof)
            {
                gravity = Gravity.DOWN;
                force.y = -6f;
            }
        }

        public void ReverseGravity(Gravity? g = null)
        {
            if (g == null)
                g = gravity == Gravity.UP ? Gravity.DOWN : Gravity.UP;

            gravity = g.Value;
        }

        public void TrimVelocity()
        {
            if (entityCollider.OnWallRight && 0 < velocity.x)
                force.x = velocity.x = 0;
            if (entityCollider.OnWallLeft && velocity.x < 0)
                force.x = velocity.x = 0;
            if (entityCollider.OnRoof && 0 < velocity.y)
                velocity.y = 0;
            if (entityCollider.OnGround && velocity.y < 0)
                velocity.y = 0;
        }
    }
}
