using LevelStructure;

public class GlobalStateMachine : FiniteStateMachine
{
      
      public SetupStateMachine SetupStateMachine { get; private set; }
      public GameplayStateMachine GameplayStateMachine { get; protected set; }
      
      public LevelCompleteStateMachine LevelCompleteStateMachine { get; protected set; }
      
      public GlobalStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller, bool isSetupSkipped)
      {
            SetupStateMachine = new SetupStateMachine(controller, isSetupSkipped)
            {
                  OnComplete = HandleSetupStateComplete
            };
            
            GameplayStateMachine = new GameplayStateMachine(actions, data, controller);

            LevelCompleteStateMachine = new LevelCompleteStateMachine();
      }

      public override void Enter()
      {
            base.Enter();
            ChangeState(SetupStateMachine);
      }

      private void HandleSetupStateComplete()
      {
            ChangeState(GameplayStateMachine);
      }
}
