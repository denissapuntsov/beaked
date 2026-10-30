using System.Collections.Generic;
using LevelStructure;

public class SetupStateMachine : FiniteStateMachine
{
        private bool _isSetupSkipped;
        
        public SetupGridState SetupGridState { get; protected set; }
        public AnimateGridState AnimateGridState { get; protected set; }
        
        public SetupStateMachine(TileGridController controller, bool isSkipped) : base()
        {
                _isSetupSkipped = isSkipped;
                
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
                LevelManager.instance.LocateSign();
        }
        
        private void HandleAnimateGridComplete()
        {
                Complete();
        }
}
