using UnityEngine.InputSystem;

namespace NewTiles.PlayerStateMachine
{
    public class BaseState
    {
        protected PlayerStateMachine _playerStateMachine;
        
        public BaseState(PlayerStateMachine machine)
        {
            _playerStateMachine = machine;
        }
        
        public virtual void Enter() { }
        public virtual void OnSpeak(InputAction.CallbackContext context) { }
        
        public virtual void OnWalk(InputAction.CallbackContext context) { }
        public virtual void OnListen(InputAction.CallbackContext context) { }
        public virtual void OnInteract(InputAction.CallbackContext context) { }

        public virtual void Exit() { }
    }
}
