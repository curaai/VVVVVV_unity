using UnityEngine;

namespace VVVVVV.World.Entity
{
    [RequireComponent(typeof(MoveController))]
    public class IntervalRepeat : MonoBehaviour
    {
        [SerializeField] public Vector2 SPEED;

        private MoveController c;

        void Start()
        {
            c = GetComponent<MoveController>();
            c.velocity = SPEED;
        }

        void FixedUpdate()
        {
            if (c.OnWallLeft || c.OnWallRight)
            {
                if (c.OnWallLeft)
                {
                    c.velocity.x = Mathf.Abs(SPEED.x);
                    c.direction = Direction.RIGHT;
                }
                else
                {
                    c.velocity.x = -Mathf.Abs(SPEED.x);
                    c.direction = Direction.LEFT;
                }
            }
            if (c.OnGround || c.OnRoof)
            {
                if (c.OnGround)
                {
                    c.velocity.y = Mathf.Abs(SPEED.y);
                    c.ReverseGravity(Gravity.UP);
                }
                else
                {
                    c.velocity.y = -Mathf.Abs(SPEED.y);
                    c.ReverseGravity(Gravity.DOWN);
                }
            }
        }
    }
}