using LevelStructure;

public class GlobalStateMachine : FiniteStateMachine
{
      private bool _isSetupSkipped = false;
      
      public SetupStateMachine SetupStateMachine { get; private set; }
      public GameplayStateMachine GameplayStateMachine { get; protected set; }
      
      public GlobalStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller, bool isSetupSkipped)
      {
            SetupStateMachine = new SetupStateMachine(controller)
            {
                  OnComplete = HandleSetupStateComplete
            };
            
            GameplayStateMachine = new GameplayStateMachine(actions, data, controller);
            
            _isSetupSkipped = isSetupSkipped;
      }

      public override void Enter()
      {
            base.Enter();
            if (_isSetupSkipped)
            {
                  ChangeState(GameplayStateMachine);
            }
            else
            {
                  ChangeState(SetupStateMachine);
            }
      }

      private void HandleSetupStateComplete()
      {
            ChangeState(GameplayStateMachine);
      }
}
