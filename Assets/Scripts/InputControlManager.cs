using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VVVVVV
{
    public abstract class IControllable : MonoBehaviour
    {
        // Control Manager don't assign next state when face dummy function
        public static void DummyMoveFunc(float x) => x += 1;
        public static void DummySpaceFunc() { return; }
        public static void DummyActionFunc() { return; }

        public enum Type
        {
            UI,
            Player,
        }
        public virtual Type controlType => throw new NullReferenceException("Null control type");

        public Action<float> OnMove = null;
        public Action OnSpace = null;
        public Action OnAction = null;

        protected bool FocusNow
        {
            get => _FocusNow;
            set
            {
                _FocusNow = value;
                if (value)
                    InputControlManager.Instance.SetFocus(this);
                else
                    InputControlManager.Instance.DeFocus(this);
            }
        }
        protected bool _FocusNow;
    }

    public class InputControlManager : Utils.Singleton<InputControlManager>
    {
        [SerializeField] private List<IControllable> controlStateList;

        void Start()
        {
            SwitchActionMap();
        }

        void SwitchActionMap()
        {
            string newmap;
            if (controlStateList[0].controlType == IControllable.Type.Player)
                newmap = "Player";
            else
                newmap = "UI";

            var playerInput = GetComponent<PlayerInput>();
            if (playerInput.currentActionMap.name != newmap)
                playerInput.SwitchCurrentActionMap(newmap);
        }

        public void OnMove(InputValue value)
        {
            var axis = value.Get<float>();

            Action<float> mainFunc = null;

            foreach (var state in controlStateList)
            {
                if (state.OnMove != null)
                {
                    mainFunc = state.OnMove;
                    break;
                }
            }

            if (mainFunc != null)
                mainFunc.Invoke(axis);
        }

        public void OnSpace()
        {
            Action mainFunc = null;

            foreach (var state in controlStateList)
            {
                if (state.OnSpace != null)
                {
                    mainFunc = state.OnSpace;
                    break;
                }
            }

            if (mainFunc != null)
                mainFunc.Invoke();
        }

        public void OnAction()
        {
            Action mainFunc = null;

            foreach (var state in controlStateList)
            {
                if (state.OnAction != null)
                {
                    mainFunc = state.OnAction;
                    break;
                }
            }

            if (mainFunc != null)
                mainFunc.Invoke();
        }

        public void OnEscape()
        {
            GameObject.Find("PanelController").GetComponent<UI.PanelController>().TogglePause();
        }

        public void SetFocus(IControllable controllable)
        {
            controlStateList.Insert(0, controllable);
            SwitchActionMap();
        }

        public void DeFocus(IControllable obj)
        {
            if (controlStateList.Contains(obj))
                controlStateList.Remove(obj);
            SwitchActionMap();
        }
    }
}