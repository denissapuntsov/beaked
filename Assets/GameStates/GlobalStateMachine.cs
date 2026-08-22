using LevelStructure;

public class GlobalStateMachine : FiniteStateMachine
{
      public GameplayStateMachine GameplayStateMachine { get; protected set; }
      
      public GlobalStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller)
      {
            GameplayStateMachine = new GameplayStateMachine(actions, data, controller);
      }

      public override void Enter()
      {
            base.Enter();
            ChangeState(GameplayStateMachine);
      }
}
