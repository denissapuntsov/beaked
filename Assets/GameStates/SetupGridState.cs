using System.Collections.Generic;
using LevelStructure;

public class SetupGridState : GameState
{
        private TileGridController _controller;
        
        public SetupGridState(TileGridController controller) : base()
        {
                _controller = controller;
        }

        public override void Enter()
        {
                _controller.Setup();
                Complete();
        }

        public override void Exit()
        {
                base.Exit();
        }
}
