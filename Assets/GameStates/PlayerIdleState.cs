using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : GameState, PlayerInputActions.IPlayerActions
{
    private PlayerInputActions _actions;
    private PlayerData _data;
    public ExitState ExitState { get; private set; }
    
    public PlayerIdleState(PlayerInputActions actions, PlayerData data) : base()
    {
        _actions = actions;
        _data = data;
    }

    public override void Enter()
    {
        _actions.Player.Enable();
        _actions.Player.SetCallbacks(this);
    }

    public void OnListen(InputAction.CallbackContext context)
    {
        ExitState = ExitState.Listening;
        Complete();
    }

    public void OnSpeak(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        if (!_data.CurrentSound) return;
        ExitState = ExitState.Speaking;
        Complete();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        ExitState = ExitState.Interacting;
        Complete();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        _data.walkInput = context.ReadValue<Vector2>();
        var walkInput = new Vector2(Mathf.Ceil(_data.walkInput.x), Mathf.Ceil(_data.walkInput.y));
        if (Mathf.Abs(walkInput.x) < 0.8f && Mathf.Abs(walkInput.y) < 0.8f)
        {
            return;
        }

        ExitState = ExitState.Walking;
        Complete();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        return;
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
    Pause,
    None
}
