using LevelStructure;
using UnityEngine;

public class PlayerStepStateMachine : FiniteStateMachine
{
        public ExitState ExitState { get; private set; }
        
        public PlayerIdleState PlayerIdleState { get; protected set; }
        public PlayerWalkState PlayerWalkState { get; protected set; }
        public PlayerSpeakState PlayerSpeakState { get; protected set; }
        public PlayerInteractState PlayerInteractState { get; protected set; }
        
        public PlayerStepStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller, MenuCanvas menuCanvas) : base()
        {
                PlayerIdleState = new PlayerIdleState(actions, data, menuCanvas)
                {
                        OnComplete = HandlePlayerIdleStateComplete
                };

                PlayerWalkState = new PlayerWalkState(actions, data, controller, menuCanvas)
                {
                        OnComplete = HandlePlayerWalkStateComplete
                };

                PlayerSpeakState = new PlayerSpeakState(data, menuCanvas)
                {
                        OnComplete = HandlePlayerSpeakStateComplete
                };

                PlayerInteractState = new PlayerInteractState(data, menuCanvas)
                {
                        OnComplete = HandlePlayerInteractStateComplete
                };
        }

        public override void Enter()
        {
                ChangeState(PlayerIdleState);
        }
        
        private void HandlePlayerIdleStateComplete()
        {
                switch (PlayerIdleState.ExitState)
                {
                        case ExitState.Walking:
                                ChangeState(PlayerWalkState);
                                break;
                        case ExitState.Speaking:
                                ChangeState(PlayerSpeakState);
                                break;
                        case ExitState.Interacting:
                                ChangeState(PlayerInteractState);
                                break;
                        case ExitState.Pause:
                                ExitState = ExitState.Pause;
                                Complete();
                                break;
                }
        }

        private void HandlePlayerWalkStateComplete()
        {
                ChangeState(PlayerIdleState);
        }

        private void HandlePlayerSpeakStateComplete()
        {
                ExitState = ExitState.Speaking;
                Complete();
        }

        private void HandlePlayerInteractStateComplete()
        {
                ExitState = ExitState.Interacting;
                Complete();
        }

        public override void Exit()
        {
                base.Exit();
                ExitState = ExitState.None;
        }
}
