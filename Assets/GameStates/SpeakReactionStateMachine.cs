using System.Collections.Generic;
using UnityEngine;

public class SpeakReactionStateMachine : FiniteStateMachine
{
    public CheckForListenersState CheckForListenersState { get; protected set; }
    
    // consider a PrimeListenersState in between?
    public TriggerListenersState TriggerListenersState { get; protected set; }
    
    public SpeakReactionStateMachine() : base()
    {
        CheckForListenersState = new CheckForListenersState()
        {
            OnComplete = HandleCheckForListenersStateComplete
        };
        TriggerListenersState = new TriggerListenersState()
        {
            OnComplete = HandleTriggerListenersStateComplete
        };
    }

    public override void Enter()
    {
        base.Enter();
        ChangeState(CheckForListenersState);
    }

    private void HandleCheckForListenersStateComplete()
    {
        if (CheckForListenersState.listenersFound.Count == 0)
        {
            Complete();
            return;
        }
        CheckForListenersState.listenersFound.CopyTo(TriggerListenersState.Listeners);
        ChangeState(TriggerListenersState);
    }

    private void HandleTriggerListenersStateComplete()
    {
        Complete();
    }
}
