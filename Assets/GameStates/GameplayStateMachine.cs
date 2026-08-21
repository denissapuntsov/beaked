using System.Collections.Generic;
using NewTiles;
using UnityEngine;


public class GameplayStateMachine : FiniteStateMachine
{
    #region States
    
    public PlayerStepStateMachine PlayerStepState { get; protected set; }
    public ReactionStepStateMachine ReactionStepState { get; protected set; }
    
    #endregion
    
    public GameplayStateMachine(PlayerInputActions actions) : base()
    {
        PlayerStepState = new PlayerStepStateMachine(actions)
        {
            OnComplete = HandlePlayerStepStateComplete
        };

        ReactionStepState = new ReactionStepStateMachine()
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
