namespace NewTiles.PlayerStateMachine
{
    public class SpeakingState : BaseState
    {
        public SpeakingState(PlayerStateMachine machine) : base(machine) { }

        public override void Enter()
        {
            PlayerData.Instance.AnimationState = PlayerAnimationState.Speaking;

            Speak();
        }

        private void Speak()
        {
            SoundManager.instance.PlaySound(PlayerData.Instance.currentSound, PlayerData.Instance.CurrentCell, OnFinishedSpeaking);
        }

        private void OnFinishedSpeaking(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
        {
            TileGrid.instance.onPlayerSoundEnded?.Invoke(PlayerData.Instance.currentSound);
            _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
        }
    }
}
