using System;
using Unity.Cinemachine.Editor;
using UnityEngine;

public abstract class FiniteStateMachine : GameState
{
    public GameState CurrentState  { get; protected set; }

    public FiniteStateMachine() : base()
    {
        
    }

    public virtual void ChangeState(GameState newState)
    {
        CurrentState?.Exit();
        
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public override void Complete()
    {
        base.Complete();
        ChangeState(null);
    }
}

public abstract class GameplayState : FiniteStateMachine
{
    protected GameplayStateMachine Machine;
    
    protected GameplayState(GameplayStateMachine machine)
    {
        Machine = machine;
    }
}
