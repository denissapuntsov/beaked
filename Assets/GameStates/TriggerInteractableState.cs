using NewTiles;
public class TriggerInteractableState : GameState
{
    public Interactable Interactable;
    
    public TriggerInteractableState() : base()
    {
        
    }

    public override void Enter()
    {
        Interactable.Interact();
        Complete();
    }
}