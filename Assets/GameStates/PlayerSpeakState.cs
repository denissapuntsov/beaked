
using DG.Tweening;
using NewTiles;
public class PlayerSpeakState : GameState
{
    private PlayerData _data;
    public PlayerSpeakState(PlayerData data) : base()
    {
        _data = data;
    }

    private bool _isAnimationOver, _isSoundOver;

    public override void Enter()
    {
        _isAnimationOver = false;
        _isSoundOver = false;
        base.Enter();
        _data.AnimationState = PlayerAnimationState.Speaking;
        SoundManager.instance.PlaySoundBird(_data.CurrentSound, _data.gameObject, HandleSoundComplete);
        Sequence animationDelay = DOTween.Sequence();
        animationDelay.AppendInterval(_data.CurrentSound.reactionDelay);
        animationDelay.OnComplete(() =>
        {
            _isAnimationOver = true;
            Complete();
        });
    }

    private void HandleSoundComplete(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        _isSoundOver = true;
        Complete();
    }

    protected override void Complete()
    {
        if (_isAnimationOver && _isSoundOver)
        {
            base.Complete();
        }
    }
}