using System;
using NewTiles;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerIdleState : GameState, PlayerInputActions.IPlayerActions
{
    private PlayerInputActions _actions;
    public ExitState ExitState { get; private set; }
    
    public PlayerIdleState(PlayerInputActions actions) : base()
    {
        _actions = actions;
    }

    public override void Enter()
    {
        _actions.Player.AddCallbacks(this);
        _actions.Player.Enable();
    }

    public void OnListen(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSpeak(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        PlayerData.Instance.walkInput = context.ReadValue<Vector2>();
        var walkInput = new Vector2(Mathf.Ceil(PlayerData.Instance.walkInput.x), Mathf.Ceil(PlayerData.Instance.walkInput.y));
        if (Mathf.Abs(walkInput.x) < 0.8f && Mathf.Abs(walkInput.y) < 0.8f)
        {
            return;
        }

        ExitState = ExitState.Walking;
        Complete();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        base.Exit();
        ExitState = ExitState.None;
        _actions.Player.RemoveCallbacks(this);
    }
}

public enum ExitState
{
    Listening,
    Speaking,
    Interacting,
    Walking,
    None
}
