using LevelStructure;
using UnityEngine;

public class CheckForInteractableState : GameState
{
        public Interactable FoundInteractable { get; private set; }
        private PlayerData _data;
        private TileGridController _controller;
        
        public CheckForInteractableState(PlayerData data, TileGridController controller) : base()
        {
                _data = data;
                _controller = controller;
        }

        public override void Enter()
        {
                var cellToCheck = GetCellInFrontOfPlayer();
                if (cellToCheck == null)
                { 
                        Complete();
                        return;
                }
                FoundInteractable = FindInteractable(cellToCheck);
                Complete();
        }

        public override void Exit()
        { 
                FoundInteractable = null;
        }

        private Cell GetCellInFrontOfPlayer()
        {
                Cell cellInFront =
                        _controller.TryGetCellAtLocation(_data.CurrentCell.GridPosition + new Vector3(1, 0, 0));
                
                return cellInFront;
        }

        private Interactable FindInteractable(Cell cell)
        {
                var foundCell = _controller.TryGetCellAtLocation(cell.GridPosition + new Vector3(0, 0.5f, 0));
                if (foundCell == null || !foundCell.OccupyingElement) return null;
                
                var interactable = foundCell.OccupyingElement.gameObject.GetComponent<Interactable>();
                return interactable ? interactable : null;
        }
}
