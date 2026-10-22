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
                MenuCanvas.instance.SetPause(true);
                Time.timeScale = 0;
        }

        public override void Exit()
        {
                _inputActions.Player.Enable();
                _inputActions.UI.Disable();
                _inputActions.UI.RemoveCallbacks(this);
                Time.timeScale = 1;
                MenuCanvas.instance.SetPause(false);
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
                if (context.started)
                {
                        MenuCanvas.instance.Navigate();
                }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
                
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
                if (!context.started) return;
                Complete();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
                
        }

        public void OnClick(InputAction.CallbackContext context)
        {
                
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
                
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
                
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
                
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
                
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
                
        }
}
