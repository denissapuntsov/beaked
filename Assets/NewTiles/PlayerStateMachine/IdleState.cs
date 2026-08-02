using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewTiles.PlayerStateMachine
{
    public class IdleState : BaseState
    {
        public IdleState(PlayerStateMachine machine) : base(machine) { }

        private Vector2 _walkInput;
        private Tween _move;

        public override void OnWalk(InputAction.CallbackContext context)
        {
            _walkInput = new Vector2(Mathf.Ceil(PlayerData.Instance.walkInput.x), Mathf.Ceil(PlayerData.Instance.walkInput.y));
            if (Mathf.Abs(_walkInput.x) < 0.8f && Mathf.Abs(_walkInput.y) < 0.8f)
            {
                return;
            }
            _playerStateMachine.CurrentState = _playerStateMachine.movingState;
        }

        public override void OnInteract(InputAction.CallbackContext context)
        {

        }

        public override void OnListen(InputAction.CallbackContext context)
        {

        }

        public override void OnSpeak(InputAction.CallbackContext context)
        {

        }
    }
}
