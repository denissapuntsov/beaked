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
                _wait = _data.transform.DOMoveX(_data.transform.position.x, 0.2f).OnComplete(Complete);
        }
}
