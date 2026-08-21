using System;
using Unity.Cinemachine.Editor;
using UnityEngine;

public abstract class FiniteStateMachine : GameState
{
    public GameState CurrentState { get; protected set; }

    public FiniteStateMachine() : base()
    {
        
    }

    public virtual void ChangeState(GameState newState)
    {
        CurrentState?.Exit();
        Debug.Log(this + ": " + newState);
        CurrentState = newState;
        CurrentState?.Enter();
    }
}
