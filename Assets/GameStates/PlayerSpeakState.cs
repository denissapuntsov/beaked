
using NewTiles;
public class PlayerSpeakState : GameState
{
    public PlayerSpeakState() : base()
    {

    }

    public override void Enter()
    {
        base.Enter();
        PlayerData.Instance.AnimationState = PlayerAnimationState.Speaking;
        SoundManager.instance.PlaySound(PlayerData.Instance.currentSound, PlayerData.Instance.CurrentCell,
            HandleSoundComplete);
    }

    private void HandleSoundComplete(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        PlayerData.Instance.AnimationState = PlayerAnimationState.Idle;
        Complete();
    }
}