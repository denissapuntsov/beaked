using System.Collections.Generic;
using LevelStructure;
using UnityEngine;

public class SpeakReactionStateMachine : FiniteStateMachine
{
    public CheckForListenersState CheckForListenersState { get; protected set; }
    
    // consider a PrimeListenersState in between?
    public TriggerListenerState TriggerListenerState { get; protected set; }
    
    public SpeakReactionStateMachine(PlayerData data, TileGridController controller) : base()
    {
        CheckForListenersState = new CheckForListenersState(data, controller)
        {
            OnComplete = HandleCheckForListenersStateComplete
        };
        TriggerListenerState = new TriggerListenerState(data)
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
        if (!CheckForListenersState.FoundListener)
        {
            Complete();
            return;
        }
        TriggerListenerState.Listener = CheckForListenersState.FoundListener;
        ChangeState(TriggerListenerState);
    }

    private void HandleTriggerListenersStateComplete()
    {
        Complete();
    }
}
