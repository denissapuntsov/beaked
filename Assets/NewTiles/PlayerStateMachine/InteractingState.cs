namespace NewTiles.PlayerStateMachine
{
    public class InteractingState : BaseState
    {
        public InteractingState(PlayerStateMachine machine) : base(machine) { }

        public override void Enter()
        {
            PlayerData.Instance.FocusedInteractable.Interact();
            _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
        }

        public override void Exit()
        {
            
        }
    }
}
