using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewTiles.PlayerStateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public IdleState idleState;
        public MovingState movingState;
        
        private BaseState _currentState;

        public BaseState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState?.Exit();
                _currentState = value;
                _currentState?.Enter();
                Debug.Log(_currentState);
            }
        }

        private void Awake()
        {
            idleState = new IdleState(this);
            movingState = new MovingState(this);
            
            CurrentState = idleState;
        }

        private void Start()
        {
            if (CurrentState == null) return;
            CurrentState.Enter();
        }

        public void OnWalk(InputAction.CallbackContext context)
        {
            PlayerData.Instance.walkInput = context.ReadValue<Vector2>();
            CurrentState.OnWalk(context);
        }
    }
}
