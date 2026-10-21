using UnityEngine;
using UnityEngine.InputSystem;

public class PauseStepState : FiniteStateMachine, PlayerInputActions.IUIActions
{
        private PlayerData _data;
        private PlayerInputActions _inputActions;
        
        public PauseStepState(PlayerData data, PlayerInputActions actions) : base()
        {
                _data = data;
                _inputActions = actions;
        }

        public override void Enter()
        {
                _inputActions.Player.Disable();
                _inputActions.UI.Enable();
                _inputActions.UI.AddCallbacks(this);
                Debug.Log("pause");
                Time.timeScale = 0;
        }

        public override void Exit()
        {
                _inputActions.Player.Enable();
                _inputActions.UI.Disable();
                _inputActions.UI.RemoveCallbacks(this);
                Time.timeScale = 1;
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
                if (!context.started) return;
                Complete();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
                throw new System.NotImplementedException();
        }
}
