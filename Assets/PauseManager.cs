using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour, PlayerInputActions.IUIActions
{
    public static PauseManager instance;
    private MenuCanvas _menuCanvas;
    
    private bool _isPaused;
    private PlayerInputActions _actions;

    private void Awake()
    {
        instance = this;
        _isPaused = false;
        _menuCanvas = GetComponent<MenuCanvas>();
    }

    public void Toggle()
    {
        _actions = GameManager.instance.Actions;
        if (!_isPaused)
        {
            TurnOn();
            return;
        }

        TurnOff();
    }

    private void TurnOn()
    {
        _isPaused = true;
        _actions.Player.Disable();
        _actions.UI.Enable();
        _actions.UI.AddCallbacks(this);

        _menuCanvas.SetPause(true);
    }

    private void TurnOff()
    {
        _isPaused = false;
        _actions.Player.Enable();
        _actions.UI.Disable();
        _actions.UI.RemoveCallbacks(this);
        
        _menuCanvas.SetPause(false);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        _menuCanvas.Navigate();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        _menuCanvas.Submit();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        TurnOff();
    }
}
