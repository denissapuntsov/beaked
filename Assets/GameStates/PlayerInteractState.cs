using DG.Tweening;
using NewTiles;

public class PlayerInteractState : GameState
{
        private PlayerData _data;
        private MenuCanvas _menuCanvas;
        
        public PlayerInteractState(PlayerData data, MenuCanvas menuCanvas) : base()
        {
                _data = data;
                _menuCanvas = menuCanvas;
        }
        
        public override void Enter()
        { 
                base.Enter();
                _menuCanvas.SetVisibility(false, true);
                _data.AnimationState = PlayerAnimationState.Interacting;
                _data.onPeckBegin?.Invoke();
                Sequence animationDelay = DOTween.Sequence();
                animationDelay.AppendInterval(0.3f);
                animationDelay.OnComplete(Complete);
        }
}
