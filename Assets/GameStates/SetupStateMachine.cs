using System.Collections.Generic;
using LevelStructure;

public class SetupStateMachine : FiniteStateMachine
{
        public SetupGridState SetupGridState { get; protected set; }
        public AnimateGridState AnimateGridState { get; protected set; }
        
        public SetupStateMachine(TileGridController controller) : base()
        {
                SetupGridState = new SetupGridState(controller)
                {
                        OnComplete = HandleSetupGridComplete
                };
                
                AnimateGridState = new AnimateGridState(controller)
                {
                        OnComplete = HandleAnimateGridComplete
                };
        }


        public override void Enter()
        {
                ChangeState(SetupGridState);
        }

        private void HandleSetupGridComplete()
        {
                ChangeState(AnimateGridState);
        }
        
        private void HandleAnimateGridComplete()
        {
                Complete();
        }
}
