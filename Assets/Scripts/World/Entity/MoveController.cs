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

    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveController : MonoBehaviour
    {
        public static readonly Vector2 MaxSpeed = new Vector2(12, 20);
        public static readonly Vector2 INERTIA = new Vector2(2.2f, 0.5f);
        private static readonly float VelocityCalibrationParameter = 25;

        [SerializeField] bool UpdateSprite;
        [SerializeField] bool ApplyFriction;

        public int CollideTargetLayer = -1;

        public bool OnAir => !(OnGround || OnRoof);
        public bool OnWallLeft { get; protected set; }
        public bool OnWallRight { get; protected set; }
        public bool OnGround { get; protected set; }
        public bool OnRoof { get; protected set; }
        protected ContactPoint2D[] lastContactPts;

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
            get => _direction; set
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

        void Awake()
        {
            if (CollideTargetLayer == -1)
                CollideTargetLayer = LayerMask.NameToLayer("wall");
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

            if ((OnWallLeft && direction == Direction.RIGHT) ||
                (OnWallRight && direction == Direction.LEFT) ||
                (!OnWallLeft && !OnWallRight))
                velocity.x = velocity.x + force.x;

            if ((OnGround && gravity == Gravity.UP) ||
                (OnRoof && gravity == Gravity.DOWN) ||
                (!OnGround && !OnRoof))
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
            if (!OnGround)
            {
                gravity = Gravity.DOWN;
                force.y = -6f;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var layer = collision.collider.gameObject.layer;
            if (layer == CollideTargetLayer)
            {
                UpdateTouchStatus(collision.contacts);
                lastContactPts = collision.contacts;
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer != CollideTargetLayer)
                return;

            if (collision.contacts.Length != lastContactPts.Length)
            {
                UpdateTouchStatus(collision.contacts);
            }

            lastContactPts = collision.contacts;
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            UpdateTouchStatus(collision.contacts);
            lastContactPts = collision.contacts;
        }

        protected void UpdateTouchStatus(ContactPoint2D[] contacts)
        {
            // disable all status 
            OnWallLeft = OnWallRight = OnGround = OnRoof = false;

            // Enable each
            foreach (var c in contacts)
            {
                if (c.collider.gameObject.layer != CollideTargetLayer)
                    continue;

                var n = c.normal;
                // sometimes not perfect one
                var nx = Math.Round(n.x, 2);
                var ny = Math.Round(n.y, 2);
                if (nx == -1f)
                {
                    OnWallRight = true;
                    if (0 < velocity.x) force.x = velocity.x = 0;
                }
                if (nx == 1)
                {
                    OnWallLeft = true;
                    if (velocity.x < 0) force.x = velocity.x = 0;
                }
                if (ny == -1)
                {
                    OnRoof = true;
                    if (0 < velocity.y) velocity.y = 0;
                }
                if (ny == 1)
                {
                    OnGround = true;
                    if (velocity.y < 0) velocity.y = 0;
                }
            }
        }

        public void ReverseGravity(Gravity? g = null)
        {
            if (g == null)
                g = gravity == Gravity.UP ? Gravity.DOWN : Gravity.UP;

            gravity = g.Value;
        }
    }
}
