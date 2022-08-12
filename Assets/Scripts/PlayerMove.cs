using System;
using UnityEngine;

namespace VVVVVV
{
    public class PlayerMove : MonoBehaviour
    {
        public static readonly float MaxSpeedX = 12f;
        public static readonly float MaxSpeedY = 20f;
        public static readonly Vector2 SPEED = new Vector2(6f, 6f);
        public static readonly Vector2 INERTIA = new Vector2(2.2f, 0.5f);

        private Animator animator => GetComponent<Animator>();
        private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

        private float x { get { return transform.position.x; } set { transform.position = new Vector3(value, transform.position.y, transform.position.z); } }
        private float y { get { return transform.position.y; } set { transform.position = new Vector3(transform.position.x, value, transform.position.z); } }

        public bool JumpNow;
        public bool OnWall;
        public bool GravityDown;
        public bool FaceRight;

        public Vector2 force; // ax,ay
        public Vector2 velocity; // vx,vy

        private int tapLeft = 0;
        private int tapRight = 0;

        private int wallLayer => LayerMask.NameToLayer("wall");

        void Start()
        {
            // TODO: Remove when implement map
            Application.targetFrameRate = 30;
            FaceRight = true;
            GravityDown = true;
        }

        void Update()
        {
            Vector2 applyFriction(Vector2 v, Vector2 inertia)
            {
                float signX = 0 < v.x ? 1 : -1;
                float signY = 0 < v.y ? 1 : -1;

                var x = Mathf.Abs(v.x);
                var y = Mathf.Abs(v.y);

                x -= inertia.x;
                y -= inertia.y;
                x = Mathf.Min(x, MaxSpeedX);
                y = Mathf.Min(y, MaxSpeedY);

                if (x < inertia.x) x = 0;
                if (y < inertia.y) y = 0;

                return new Vector2(x * signX, y * signY);
            }

            void TapMove()
            {
                if (force.x < 0) tapLeft++;
                else
                {
                    if (0 < tapLeft && velocity.x < 0) velocity.x = 0;
                    tapLeft = 0;
                }

                if (0 < force.x) tapRight++;
                else
                {
                    if (0 < tapRight && 0 < velocity.x) velocity.x = 0;
                    tapRight = 0;
                }
            }

            force.x = 0;
            var hInput = Input.GetAxis("Horizontal");
            if (hInput != 0)
            {
                FaceRight = 0 < hInput;
                force.x = FaceRight ? SPEED.x : -SPEED.x;
            }
            TapMove();

            var vInput = Input.GetAxis("Vertical");
            if (vInput != 0)
            {
                if (OnWall && !JumpNow)
                {
                    GravityDown = !GravityDown;
                    velocity.y = GravityDown ? -8 : 8;
                    force.y = GravityDown ? -SPEED.y : SPEED.y;
                    JumpNow = true;
                }
            }
            if (OnWall && !JumpNow)
            {
                force.y = 0;
                velocity.y = 0;
            }

            velocity += force;
            velocity = applyFriction(velocity, INERTIA);

            x += velocity.x;
            y += velocity.y;

            animator.SetBool("MoveNow", velocity.x != 0);
            animator.SetBool("JumpNow", JumpNow);

            spriteRenderer.flipX = !FaceRight;
            spriteRenderer.flipY = !GravityDown;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var layer = collider.gameObject.layer;
            if (layer == wallLayer)
            {
                OnWall = true;
                JumpNow = false;
            }
        }
        private void OnTriggerExit2D(Collider2D collider)
        {
            var layer = collider.gameObject.layer;
            if (layer == wallLayer)
            {
                OnWall = false;
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.layer != wallLayer)
                return;

            // TODO: Adjust velocity before collide (overlap animation bug)
            var overlapDist = GetComponent<BoxCollider2D>().Distance(collider);
            var diff = overlapDist.pointA.y - overlapDist.pointB.y;
            diff += diff < 0 ? 0.1f : -0.1f;

            y -= diff;
        }
    }
}
