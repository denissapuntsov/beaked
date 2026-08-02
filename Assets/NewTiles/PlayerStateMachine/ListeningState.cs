using UnityEngine.InputSystem;

namespace NewTiles.PlayerStateMachine
{
    public class ListeningState : BaseState
    {
        public ListeningState(PlayerStateMachine machine) : base(machine) { }

        public override void Enter()
        {
            PlayerData.Instance.currentSound = PlayerData.Instance.FocusedSpeaker.SoundItem;
            // play animations and sounds, and then go back to Idle
            _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
        }
    }
}
