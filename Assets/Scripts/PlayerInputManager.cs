using System.Linq;
using UnityEngine;
using VVVVVV.World.Entity;

namespace VVVVVV
{
    [RequireComponent(typeof(MoveController))]
    public class PlayerInputManager : MonoBehaviour
    {
        public static readonly KeyCode[] VerticalKeyList = new KeyCode[] { KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.Space };
        private int tapLeft = 0;
        private int tapRight = 0;

        void Update()
        {
            var controller = GetComponent<MoveController>();
            var force = controller.force;
            var velocity = controller.velocity;

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

            controller.force.x = 0;
            var hInput = Input.GetAxis("Horizontal");
            if (hInput != 0)
            {
                controller.direction = hInput < 0 ? Direction.LEFT : Direction.RIGHT;
                controller.force.x = controller.direction == Direction.RIGHT ? MoveController.SPEED.x : -MoveController.SPEED.x;
            }

            // use key down to avoid repeat gravity switching 
            if (VerticalKeyList.Select(Input.GetKeyDown).Contains(true))
            {
                if (!controller.OnAir)
                {
                    controller.ReverseGravity();
                    force.y = MoveController.SPEED.y;
                    controller.force.y = controller.gravity == Gravity.DOWN ? -force.y : force.y;
                }
            }

            TapMove();
        }
    }
}