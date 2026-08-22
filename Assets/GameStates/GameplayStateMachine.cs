using System.Collections.Generic;
using LevelStructure;
using UnityEngine;


public class GameplayStateMachine : FiniteStateMachine
{
    #region States
    public PlayerStepStateMachine PlayerStepState { get; protected set; }
    public ReactionStepStateMachine ReactionStepState { get; protected set; }
    
    #endregion
    
    public GameplayStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller) : base()
    {
        PlayerStepState = new PlayerStepStateMachine(actions, data, controller)
        {
            OnComplete = HandlePlayerStepStateComplete
        };

        ReactionStepState = new ReactionStepStateMachine(data, controller)
        {
            OnComplete = HandleReactionStepComplete
        };
    }

    public override void Enter()
    {
        base.Enter();
        ChangeState(PlayerStepState);
    }

    #region Handlers

    private void HandlePlayerStepStateComplete()
    {
        ReactionStepState.EntryState = PlayerStepState.ExitState;
        ChangeState(ReactionStepState);
    }

    private void HandleReactionStepComplete()
    {
        ChangeState(PlayerStepState);
    }
    
    #endregion
}
