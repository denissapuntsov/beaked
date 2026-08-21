using NewTiles;
using Cell = NewTiles.Cell;
using UnityEngine;

public class CheckForInteractableState : GameState
{
        public Interactable FoundInteractable { get; private set; }
        
        public CheckForInteractableState() : base()
        {
                
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

        private NewTiles.Cell GetCellInFrontOfPlayer()
        {
                NewTiles.Cell cellInFront;
                TileGrid.instance.GetCellAt(
                        PlayerData.Instance.CurrentCell.gridPosition + new Vector3(1, 0, 0),
                        out cellInFront);
                
                return cellInFront;
        }

        private Interactable FindInteractable(NewTiles.Cell cell)
        {
                TileGrid.instance.GetCellAt(cell.gridPosition + new Vector3(0, 0.5f, 0), out var foundCell);
                if (foundCell == null || !foundCell.occupyingElement) return null;
                
                var interactable = foundCell.occupyingElement.gameObject.GetComponent<Interactable>();
                return interactable ? interactable : null;
        }
}
