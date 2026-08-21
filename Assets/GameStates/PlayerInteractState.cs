using NewTiles;

public class PlayerInteractState : GameState
{
        public PlayerInteractState() : base()
        {
                
        }
        
        public override void Enter()
        { 
                base.Enter();
                Complete();
        }
}
