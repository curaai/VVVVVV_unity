using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VVVVVV
{
    public abstract class IControllable : MonoBehaviour
    {
        public enum Type
        {
            Player,
            UI,
        }
        public Type controlType = IControllable.Type.Player;

        public Action<float> OnMove = null;
        public Action OnSpace = null;
        public Action OnAction = null;
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
            if (controlStateList[0].controlType == IControllable.Type.Player)
                GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            else
                GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
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

            if (mainFunc == null)
                throw new ArgumentNullException("Can't fetch OnMove Funtion");

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

            if (mainFunc == null)
                throw new ArgumentNullException("Can't fetch OnSpace Funtion");

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

            if (mainFunc == null)
                throw new ArgumentNullException("Can't fetch OnAction Funtion");

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

        public void DeFocus()
        {
            controlStateList.RemoveAt(0);
            SwitchActionMap();
        }
    }
}