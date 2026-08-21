using NewTiles;
using UnityEngine;
using UnityEngine.XR;

public class InteractReactionStateMachine : FiniteStateMachine
{
    public CheckForInteractableState CheckForInteractableState { get; protected set; }
    public TriggerInteractableState TriggerInteractableState { get; protected set; }
    
    public InteractReactionStateMachine() : base()
    {
        CheckForInteractableState = new CheckForInteractableState()
        {
            OnComplete = HandleCheckForInteractableComplete
        };

        TriggerInteractableState = new TriggerInteractableState()
        {
            OnComplete = HandleTriggerInteractableComplete
        };
    }

    public override void Enter()
    {
        base.Enter();
        ChangeState(CheckForInteractableState);
    }

    private void HandleCheckForInteractableComplete()
    {
        if (!CheckForInteractableState.FoundInteractable)
        {
            Complete();
            return;
        }
        TriggerInteractableState.Interactable = CheckForInteractableState.FoundInteractable;
        ChangeState(TriggerInteractableState);
    }

    private void HandleTriggerInteractableComplete()
    {
        Complete();
    }
}
