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
    
    private MenuCanvas _menuCanvas;
    
    public GameplayStateMachine(PlayerInputActions actions, PlayerData data, TileGridController controller, MenuCanvas menuCanvas) : base()
    {
        PlayerStepState = new PlayerStepStateMachine(actions, data, controller, menuCanvas)
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
        
        _menuCanvas = menuCanvas;
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
        
        _menuCanvas.SetVisibility(false, false);

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
        _menuCanvas.SetVisibility(false, false);
        // if the player has a new sound, react to acquisition
        ChangeState(PlayerReactionStepState);
    }

    private void HandlePlayerReactionStepComplete()
    {
        if (LevelManager.instance.IsWin)
        {
            Complete();
            return;
        }
        ChangeState(PlayerStepState);
    }

    private void HandlePauseStepComplete()
    {
        ChangeState(PlayerStepState);
    }
    
    #endregion
}
