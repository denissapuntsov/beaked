using System;
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
                var front = _data.orientation switch
                {
                        Orientation.North => new Vector3(0, 0, 1),
                        Orientation.East => new Vector3(1, 0, 0),
                        Orientation.South => new Vector3(0, 0, -1),
                        Orientation.West => new Vector3(-1, 0, 0),
                        _ => throw new ArgumentOutOfRangeException()
                };
                var foundCell = _controller.TryGetCellAtLocation(_data.CurrentCell.GridPosition + front);
                return foundCell;
        }

        private Interactable FindInteractable(Cell cell)
        {
                var foundCell = _controller.TryGetCellAtLocation(cell.GridPosition + new Vector3(0, 0.5f, 0));
                var element = _controller.GetElementFromCell(foundCell);
                if (foundCell == null || !element) return null;
                
                var interactable = element.GetComponent<Interactable>();
                return interactable ? interactable : null;
        }
}
