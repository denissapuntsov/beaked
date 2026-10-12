using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class GameState
{
    public Action OnEnter, OnComplete;
    
    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    /*public virtual void Update()
    {
    }*/

    public virtual void Exit()
    {
        
    }

    protected virtual void Complete()
    {
        OnComplete?.Invoke();
    }
}
