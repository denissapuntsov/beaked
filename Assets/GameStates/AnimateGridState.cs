using System;
using System.Collections.Generic;
using LevelStructure;

public class AnimateGridState : FiniteStateMachine
{
        private TileGridController _controller;

        private Action OnGridAnimationComplete;
        
        public AnimateGridState(TileGridController controller) : base()
        {
                _controller = controller;
                OnGridAnimationComplete = HandleGridAnimationComplete;
        }

        public override void Enter()
        {
                base.Enter();
                _controller.Animate(OnGridAnimationComplete);
        }

        private void HandleGridAnimationComplete()
        {
                Complete();
        }
}