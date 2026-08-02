using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewTiles.PlayerStateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public IdleState IdleState;
        public MovingState MovingState;
        public SpeakingState SpeakingState;
        
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
            IdleState = new IdleState(this);
            MovingState = new MovingState(this);
            SpeakingState = new SpeakingState(this);
            
            CurrentState = IdleState;
        }

        /*private void Start()
        {
            if (CurrentState == null) return;
            CurrentState.Enter();
        }*/

        public void OnWalk(InputAction.CallbackContext context)
        {
            PlayerData.Instance.walkInput = context.ReadValue<Vector2>();
            CurrentState.OnWalk(context);
        }

        public void OnListen(InputAction.CallbackContext context)
        {
            if (!PlayerData.Instance.FocusedSpeaker) return;
            CurrentState.OnListen(context);
        }

        public void OnSpeak(InputAction.CallbackContext context)
        {
            if (!PlayerData.Instance.currentSound) return;
            CurrentState.OnSpeak(context);
        }
    }
}
