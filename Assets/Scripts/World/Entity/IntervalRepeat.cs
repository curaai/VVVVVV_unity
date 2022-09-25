using UnityEngine;

namespace VVVVVV.World.Entity
{
    [RequireComponent(typeof(MoveController))]
    public class IntervalRepeat : MonoBehaviour
    {
        [SerializeField] public Vector2 SPEED;

        private MoveController m;
        private EntityCollider c;

        void Awake()
        {
            m = GetComponent<MoveController>();
            m.velocity = SPEED;
            c = GetComponent<EntityCollider>();
        }

        public void ReverseVelocity()
        {
            if (c.OnWallLeft || c.OnWallRight)
            {
                if (c.OnWallLeft)
                {
                    m.velocity.x = Mathf.Abs(SPEED.x);
                    m.direction = Direction.RIGHT;
                }
                else
                {
                    m.velocity.x = -Mathf.Abs(SPEED.x);
                    m.direction = Direction.LEFT;
                }
            }
            if (c.OnGround || c.OnRoof)
            {
                if (c.OnGround)
                {
                    m.velocity.y = Mathf.Abs(SPEED.y);
                    m.ReverseGravity(Gravity.UP);
                }
                else
                {
                    m.velocity.y = -Mathf.Abs(SPEED.y);
                    m.ReverseGravity(Gravity.DOWN);
                }
            }
        }
    }
}