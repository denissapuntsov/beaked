using Interactable_States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractableStateMachine : FiniteStateMachine
{
    public IdleState IdleState { get; protected set; }
    public AlertState AlertState { get; protected set; }
    public MovingState MovingState { get; protected set; }

    public InteractableStateMachine() : base()
    {
        IdleState = new IdleState()
        {
            OnEnter = HandleEnterIdleState,
        };
        
        AlertState = new AlertState()
        {
            OnEnter = HandleEnterAlertState,
        };
    }
    
    public override void Enter()
    {
        base.Enter();
        ChangeState(IdleState);
    }

    private void HandleEnterIdleState()
    {
        
    }

    private void HandleEnterAlertState()
    {
        
    }

    private void HandleEnterActiveState()
    {
        
    }

    private void HandleEnterMovingState()
    {
        
    }
}
