using DG.Tweening;
using LevelStructure;

public class PlayerReactionStepState : FiniteStateMachine
{
    private PlayerData _data;
    private TileGridController _controller;
    
    public PlayerReactionStepState(PlayerData data, TileGridController controller) : base() 
    {
            _data = data;
            _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();

        if (_data.soundHistory.Contains(_data.CurrentSound) || !_data.CurrentSound)
        {
            Complete();
        }
        else
        {
            _data.soundHistory.Add(_data.CurrentSound);
            _data.AnimationState = PlayerAnimationState.Learning;
            Sequence animationDelay = DOTween.Sequence();
            animationDelay.AppendInterval(0.85f);
            animationDelay.OnComplete(Complete);
        }
    }
}