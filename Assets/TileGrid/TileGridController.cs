using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridController : MonoBehaviour
    {
        private TileGridModel _model;
        private TileGridView _view;
        private Dictionary<Cell, GridElement> _cellToElementMap = new();

        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            _model = new TileGridModel();
            _view = new TileGridView();
            
            _view.GridElements = FindObjectsByType<GridElement>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None).ToList();

            foreach (var gridElement in _view.GridElements)
            {
                AssignCells(gridElement);
            }
            
            UnlockExposedCells();
        }

        private void AssignCells(GridElement gridElement)
        {
            var cells = new List<Cell>();
            for (int x = 0; x < gridElement.GridSize.x; x++)
            {
                for (float y = 0; y < gridElement.GridSize.y; y += 0.5f)
                {
                    // I don't like using a triple for loop, yeesh
                    for (int z = 0; z < gridElement.GridSize.z; z++)
                    {
                        var newCell = new Cell()
                        {
                            GridPosition = gridElement.transform.position + new Vector3(x, y + 1, z),
                            IsAccessible = false,
                        };
                        _model.GridLocationToCell.Add(newCell.GridPosition, newCell);
                        _cellToElementMap.Add(newCell, gridElement);
                    }
                }
            }
        }

        private void UnlockExposedCells()
        {
            foreach (var cell in _cellToElementMap.Keys)
            {
                cell.IsAccessible = _model.TryGetCellAtLocation(cell.GridPosition + new Vector3(0, 0.5f, 0)) == null;
                /*if (cell.IsAccessible)
                {
                    Debug.Log($"{cell.GridPosition} is accessible!");
                }*/
            }
        }
    }
}
