using LevelStructure;
using NewTiles;
using UnityEngine;
using UnityEngine.XR;

public class InteractReactionStateMachine : FiniteStateMachine
{
    public CheckForInteractableState CheckForInteractableState { get; protected set; }
    public TriggerInteractableState TriggerInteractableState { get; protected set; }
    
    public  InteractReactionStateMachine(PlayerData data, TileGridController controller) : base()
    {
        CheckForInteractableState = new CheckForInteractableState(data, controller)
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
            Debug.Log("nothing");
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
