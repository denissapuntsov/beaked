using DG.Tweening;
using NewTiles;

public class PlayerInteractState : GameState
{
        private PlayerData _data;
        Tweener _wait;
        
        public PlayerInteractState(PlayerData data) : base()
        {
                _data = data;
        }
        
        public override void Enter()
        { 
                base.Enter();
                _data.AnimationState = PlayerAnimationState.Interacting;
                Sequence animationDelay = DOTween.Sequence();
                animationDelay.AppendInterval(0.3f);
                animationDelay.OnComplete(Complete);
        }
}
