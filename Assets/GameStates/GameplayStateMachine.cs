using System.Collections.Generic;
using LevelStructure;
using UnityEngine;


public class GameplayStateMachine : FiniteStateMachine
{
    #region States
    public PlayerStepStateMachine PlayerStepState { get; protected set; }
    public WorldReactionStepState WorldReactionStepState { get; protected set; }
    
    public PlayerReactionStepState PlayerReactionStepState { get; protected set; }
    
    public PauseStepState PauseStepState { get; protected set; }
    
    #endregion
    
    public GameplayStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller) : base()
    {
        PlayerStepState = new PlayerStepStateMachine(actions, data, controller)
        {
            OnComplete = HandlePlayerStepStateComplete
        };

        WorldReactionStepState = new WorldReactionStepState(data, controller)
        {
            OnComplete = HandleWorldReactionStepComplete
        };

        PlayerReactionStepState = new PlayerReactionStepState(data, controller)
        {
            OnComplete = HandlePlayerReactionStepComplete
        };

        PauseStepState = new PauseStepState(data, actions)
        {
            OnComplete = HandlePauseStepComplete
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
        // analyse the world to see if our action has reached a Peckable or a Listener

        if (PlayerStepState.ExitState == ExitState.Pause)
        {
            ChangeState(PauseStepState);
        }
        else
        {
            WorldReactionStepState.EntryState = PlayerStepState.ExitState;
            ChangeState(WorldReactionStepState);
        }
    }
    
    private void HandleWorldReactionStepComplete()
    {
        // if the player has a new sound, react to acquisition
        ChangeState(PlayerReactionStepState);
    }

    private void HandlePlayerReactionStepComplete()
    {
        // have we reached the end?
        ChangeState(PlayerStepState);
    }

    private void HandlePauseStepComplete()
    {
        ChangeState(PlayerStepState);
    }
    
    #endregion
}
