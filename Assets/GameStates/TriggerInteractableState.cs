using System.Collections.Generic;
using NewTiles;
public class TriggerInteractableState : GameState
{
    public Interactable Interactable;
    
    public TriggerInteractableState() : base()
    {
        
    }

    public override void Enter()
    {
        Interactable.OnComplete = HandleInteractableComplete;
        Interactable.OnPeck();
    }

    private void HandleInteractableComplete(ChainableAction interactable)
    {
        Complete();
    }
}