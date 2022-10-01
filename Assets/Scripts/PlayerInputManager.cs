using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VVVVVV.World.Entity;

namespace VVVVVV
{
    [RequireComponent(typeof(MoveController))]
    public class PlayerInputManager : IControllable
    {
        public static readonly Vector2 SPEED = new Vector2(6f, 6f);

        [SerializeField] AudioClip onGroundSound;
        [SerializeField] AudioClip onRoofSound;

        MoveController controller;
        EntityCollider entityCollider;

        void Awake()
        {
            controller = GetComponent<MoveController>();
            entityCollider = GetComponent<EntityCollider>();

            controlType = IControllable.Type.Player;
            OnMove += Move;
            OnSpace += Space;
        }

        void Move(float axis)
        {
            if (axis == 0)
            {
                controller.force.x = 0;
            }
            else if (axis < 0)
            {
                controller.direction = Direction.LEFT;
                controller.force.x = -Mathf.Abs(SPEED.x);
            }
            else
            {
                controller.direction = Direction.RIGHT;
                controller.force.x = Mathf.Abs(SPEED.x);
            }
        }

        void Space()
        {
            if (entityCollider.OnGround)
            {
                SoundManager.Instance.PlayEffect(onGroundSound);

                controller.ReverseGravity(Gravity.UP);
                controller.force.y = Mathf.Abs(SPEED.y);
            }
            else if (entityCollider.OnRoof)
            {
                SoundManager.Instance.PlayEffect(onRoofSound);

                controller.ReverseGravity(Gravity.DOWN);
                controller.force.y = -Mathf.Abs(SPEED.y);
            }
        }
    }
}