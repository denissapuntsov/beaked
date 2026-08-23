using System;
using System.Collections.Generic;
using LevelStructure;

public class AnimateGridState : GameState
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
                _controller.AnimateGridSetup(OnGridAnimationComplete);
        }

        private void HandleGridAnimationComplete()
        {
                Complete();
        }
}