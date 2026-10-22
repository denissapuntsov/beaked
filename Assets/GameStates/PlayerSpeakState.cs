
using DG.Tweening;
using NewTiles;
public class PlayerSpeakState : GameState
{
    private PlayerData _data;
    private MenuCanvas _menuCanvas;
    public PlayerSpeakState(PlayerData data, MenuCanvas menuCanvas) : base()
    {
        _data = data;
        _menuCanvas = menuCanvas;
    }

    private bool _isAnimationOver, _isSoundOver;

    public override void Enter()
    {
        _menuCanvas.SetVisibility(true, false);
        _isAnimationOver = false;
        _isSoundOver = false;
        base.Enter();
        _data.AnimationState = PlayerAnimationState.Speaking;
        SoundManager.instance.PlaySound(_data.currentSound, _data.gameObject, HandleSoundComplete);
        Sequence animationDelay = DOTween.Sequence();
        animationDelay.AppendInterval(_data.currentSound.reactionDelay);
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