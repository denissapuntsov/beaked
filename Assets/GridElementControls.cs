public abstract class GridElementControls : ChainableAction
{
        protected PlayerData Data;
    
        protected virtual void Start()
        {
                Data = FindAnyObjectByType<PlayerData>();
        }
        
        public virtual void ReactToPeck() {}
}
