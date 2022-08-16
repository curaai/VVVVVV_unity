using System;
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

    // [RequireComponent(typeof(World.Entity.GravityController))]
    public class MoveController : MonoBehaviour
    {

        public static readonly Vector2 MaxSpeed = new Vector2(12, 20);
        public static readonly Vector2 SPEED = new Vector2(6f, 6f);
        public static readonly Vector2 INERTIA = new Vector2(2.2f, 0.5f);
        protected static int wallLayer => LayerMask.NameToLayer("wall");

        [SerializeField] (bool, bool) UpdateSprite;

        protected Animator animator => GetComponent<Animator>();
        protected SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

        public bool OnAir => !(OnGround || OnRoof);
        [SerializeField] protected bool OnWallLeft;
        [SerializeField] protected bool OnWallRight;
        [SerializeField] protected bool OnGround;
        [SerializeField] protected bool OnRoof;
        ContactPoint2D[] lastContactPts;

        public Gravity gravity;
        public Direction direction;

        public Vector2 force;
        public Vector2 velocity;

        void Update()
        {
            Vector2 applyFriction(Vector2 v)
            {
                float signX = 0 < v.x ? 1 : -1;
                float signY = 0 < v.y ? 1 : -1;

                var x = Mathf.Abs(v.x);
                var y = Mathf.Abs(v.y);

                x -= INERTIA.x;
                y -= INERTIA.y;
                x = Mathf.Min(x, MaxSpeed.x);
                y = Mathf.Min(y, MaxSpeed.y);

                if (x < INERTIA.x) x = 0;
                if (y < INERTIA.y) y = 0;

                return new Vector2(x * signX, y * signY);
            }

            if ((OnWallLeft && direction == Direction.RIGHT ||
                 OnWallRight && direction == Direction.LEFT) ||
                (!OnWallLeft && !OnWallRight))
                velocity.x = velocity.x + force.x;

            if ((OnGround && gravity == Gravity.UP ||
                 OnRoof && gravity == Gravity.DOWN) ||
                (!OnGround && !OnRoof))
                velocity.y = velocity.y + force.y;

            velocity = applyFriction(velocity);
            // Check Collision
            // Replace to RigidBody 2d? 
            transform.position += new Vector3(velocity.x, velocity.y, 0);

            animator?.SetBool("MoveNow", velocity != Vector2.zero);
            animator?.SetBool("JumpNow", OnAir);

            if (UpdateSprite.Item1)
                spriteRenderer.flipX = direction == Direction.RIGHT;
            if (UpdateSprite.Item2)
                spriteRenderer.flipY = gravity == Gravity.UP;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision Enter");
            var layer = collision.collider.gameObject.layer;
            if (layer == wallLayer)
            {
                UpdateTouchStatus(collision.contacts);
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            var layer = collision.collider.gameObject.layer;
            if (layer == wallLayer)
            {
                foreach (var contact in lastContactPts)
                {
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer != wallLayer)
                return;

            UpdateTouchStatus(collision.contacts);

            // // TODO: Adjust velocity before collide (overlap animation bug)
            // var overlapDist = GetComponent<BoxCollider2D>().Distance(collider);
            // var diff = overlapDist.pointA.y - overlapDist.pointB.y;
            // diff += diff < 0 ? 0.1f : -0.1f;

            // y -= diff;
        }

        void UpdateTouchStatus(ContactPoint2D[] contacts)
        {
            foreach (var contact in contacts)
            {
                Debug.Log(contact.normal);
                var nx = contact.normal.x;
                var ny = contact.normal.y;
                if (nx == -1f)
                {
                    OnWallLeft = true;
                    if (velocity.x < 0) force.x = velocity.x = 0;
                }
                if (nx == 1f)
                {
                    OnWallRight = true;
                    if (0 < velocity.x) force.x = velocity.x = 0;
                }
                if (ny == -1)
                {
                    OnRoof = true;
                    if (0 < velocity.y) force.y = velocity.y = 0;
                }
                if (ny == 1)
                {
                    OnGround = true;
                    if (velocity.y < 0) force.y = velocity.y = 0;
                }

                var pt = GetComponent<Rigidbody2D>().ClosestPoint(contact.point);
                var diff = pt - contact.point;
                if (diff.x < 0) OnWallLeft = false;
                if (0 < diff.x) OnWallRight = false;
                if (0 < diff.y) OnGround = false;
                if (diff.y < 0) OnRoof = false;
            }
            lastContactPts = contacts;
        }

        public void ReverseGravity(Gravity? g = null)
        {
            if (g == null)
                g = gravity == Gravity.UP ? Gravity.DOWN : Gravity.UP;

            gravity = g.Value;
        }
    }
}
