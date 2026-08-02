namespace NewTiles.PlayerStateMachine
{
    public class SpeakingState : BaseState
    {
        public SpeakingState(PlayerStateMachine machine) : base(machine) { }

        public override void Enter()
        {
            TileGrid.instance.onPlayerSoundStarted?.Invoke(PlayerData.Instance.currentSound);
            PlayerData.Instance.currentSound.audioEvent.Post(_playerStateMachine.gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnFinishedSpeaking);
        }
        
        private void OnFinishedSpeaking(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
        {
            TileGrid.instance.onPlayerSoundEnded?.Invoke(PlayerData.Instance.currentSound);
            _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
        }
    }
}
