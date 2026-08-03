namespace NewTiles.PlayerStateMachine
{
    public class SpeakingState : BaseState
    {
        public SpeakingState(PlayerStateMachine machine) : base(machine) { }

        public override void Enter()
        {
            TileGrid.instance.onPlayerSoundStarted?.Invoke(PlayerData.Instance.currentSound);
            PopupManager.instance.ShowPopup(_playerStateMachine.gameObject, "!");
            PlayerData.Instance.currentSound.audioEvent.Post(_playerStateMachine.gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnFinishedSpeaking);
        }
        
        private void OnFinishedSpeaking(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
        {
            TileGrid.instance.onPlayerSoundEnded?.Invoke(PlayerData.Instance.currentSound);
            PopupManager.instance.HidePopup(_playerStateMachine.gameObject);
            _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
        }
    }
}
