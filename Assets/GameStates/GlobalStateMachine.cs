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

            GameplayStateMachine = new GameplayStateMachine(actions, data, controller)
            {
                  OnComplete = HandleGameplayerStateComplete
            };

            LevelCompleteStateMachine = new LevelCompleteStateMachine()
            {
                  OnComplete = HandleLevelCompleteStateComplete
            };
      }

      public override void Enter()
      {
            base.Enter();
            ChangeState(SetupStateMachine);
      }

      private void HandleSetupStateComplete()
      {
            SoundManager.instance.SetMusic(MusicMode.Gameplay);
            ChangeState(GameplayStateMachine);
      }

      private void HandleGameplayerStateComplete()
      {
            ChangeState(LevelCompleteStateMachine);
      }

      private void HandleLevelCompleteStateComplete()
      {
            Complete();
      }
}
