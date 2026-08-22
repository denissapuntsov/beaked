
using NewTiles;
public class PlayerSpeakState : GameState
{
    private PlayerData _data;
    public PlayerSpeakState(PlayerData data) : base()
    {
        _data = data;
    }

    public override void Enter()
    {
        base.Enter();
        _data.AnimationState = PlayerAnimationState.Speaking;
        SoundManager.instance.PlaySound(_data.currentSound, _data.gameObject, HandleSoundComplete);
    }

    private void HandleSoundComplete(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        _data.AnimationState = PlayerAnimationState.Idle;
        Complete();
    }
}