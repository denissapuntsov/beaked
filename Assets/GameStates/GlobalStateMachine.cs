using LevelStructure;
using UnityEngine.SceneManagement;

public class GlobalStateMachine : FiniteStateMachine
{
      
      public SetupStateMachine SetupStateMachine { get; private set; }
      public GameplayStateMachine GameplayStateMachine { get; protected set; }
      public LevelCompleteStateMachine LevelCompleteStateMachine { get; protected set; }
      
      public GlobalStateMachine(
            PlayerInputActions actions, 
            PlayerData data, 
            TileGridController controller, 
            bool isSetupSkipped, 
            MenuCanvas menuCanvas)
      {
            SetupStateMachine = new SetupStateMachine(controller, isSetupSkipped)
            {
                  OnComplete = HandleSetupStateComplete
            };

            GameplayStateMachine = new GameplayStateMachine(actions, data, controller)
            {
                  OnComplete = HandleGameplayerStateComplete
            };

            LevelCompleteStateMachine = new LevelCompleteStateMachine(controller)
            {
                  OnComplete = HandleLevelCompleteStateComplete
            };
      }

      public override void Enter()
      {
            base.Enter();
            if (SceneManager.GetActiveScene().name 
                is not "MainMenu" 
                and not "Ch1Transition" 
                and not "Ch2Transition" 
                and not "Ch3Transition" 
                and not "Credits")
            {
                  ChangeState(SetupStateMachine);
            }
      }

      private void HandleSetupStateComplete()
      {
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
