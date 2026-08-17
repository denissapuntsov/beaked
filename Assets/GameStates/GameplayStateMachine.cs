using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameplayStateMachine : FiniteStateMachine
{
    protected List<GameObject> ListenersToTrigger;
    
    public PlayerIdleState PlayerIdleState { get; protected set; }
    public PlayerWalkState PlayerWalkState { get; protected set; }
    public ReactionStateMachine ReactionState { get; protected set; }
    public GameplayStateMachine(PlayerInputActions actions) : base()
    {
        PlayerIdleState = new PlayerIdleState(actions)
        {
            OnComplete = HandlePlayerIdleStateComplete
        };

        PlayerWalkState = new PlayerWalkState(actions)
        {
            OnComplete = HandlePlayerWalkStateComplete
        };
        
        ReactionState = new ReactionStateMachine(ListenersToTrigger)
        {
            OnComplete = HandleReactionStepComplete
        };
    }

    public override void ChangeState(GameState newState)
    {
        base.ChangeState(newState);
        Debug.Log("TEST: " + newState);
    }

    public override void Enter()
    {
        base.Enter();
        ChangeState(PlayerIdleState);
    }

    protected void HandlePlayerIdleStateComplete()
    {
        switch (PlayerIdleState.ExitState)
        {
            case ExitState.Walking:
                ChangeState(PlayerWalkState);
                break;
        }
    }
    
    protected void HandleReactionStepComplete()
    {
        ChangeState(PlayerIdleState);
    }

    protected void HandlePlayerWalkStateComplete()
    {
        ChangeState(PlayerIdleState);
    }
}
