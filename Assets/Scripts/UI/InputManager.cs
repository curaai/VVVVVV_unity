using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VVVVVV.UI
{
    public interface IInputtable
    {
        // public void OnInputFocus();
        // public void OnDefocusInput();
        public void OnMoveInput(float v);
        public void OnActionInput();
        public void OnSpaceInput();
    }

    public class InputManager : Utils.Singleton<InputManager>
    {
        private readonly string UI_INPUT_MAP = "UI";

        [SerializeField] PlayerInput playerInput;

        private Stack<IInputtable> inputUIStack = new Stack<IInputtable>();
        private string previousMap;

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (inputUIStack.Count == 0) return;
            if (!ctx.performed) return;

            inputUIStack.Peek().OnMoveInput(ctx.ReadValue<float>());
        }

        public void Focus(IInputtable i)
        {
            var curmap = playerInput.currentActionMap.name;
            if (curmap != UI_INPUT_MAP)
            {
                previousMap = curmap;
                playerInput.currentActionMap.Disable();
                playerInput.SwitchCurrentActionMap(UI_INPUT_MAP);
                playerInput.currentActionMap.Enable();
                Debug.Log($"INPUT: Map changed to {UI_INPUT_MAP}");
            }

            inputUIStack.Push(i);
        }
        public void Defocus()
        {
            if (inputUIStack.Count == 0) return;

            inputUIStack.Pop();

            if (inputUIStack.Count == 0)
            {
                playerInput.currentActionMap.Disable();
                playerInput.SwitchCurrentActionMap(previousMap);
                playerInput.currentActionMap.Enable();
                Debug.Log($"INPUT: Map changed to {previousMap}");
            }
        }
    }
}