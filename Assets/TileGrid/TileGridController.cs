using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridController : MonoBehaviour
    {
        private TileGridModel _model;
        private TileGridView _view;
        private PlayerData _data;
        private Dictionary<Cell, GridElement> _cellToElementMap = new();
        
        public List<GridElement> GridElements => _view.GridElements;

        private void Awake()
        {
            _data = FindAnyObjectByType<PlayerData>();
        }

        public void Setup()
        {
            SetupGrid();
            AssignPlayerCell();
        }

        public void AnimateGridSetup(Action onComplete)
        {
            _view.AnimateGrid(onComplete);
        }

        private void SetupGrid()
        {
            _model = new TileGridModel();
            _view = new TileGridView();

            _view.GridElements = FindObjectsByType<GridElement>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None).ToList();

            foreach (var gridElement in _view.GridElements)
            {
                gridElement.OnDestroyOrDisable = RemoveElement;
                AssignCells(gridElement);
            }

            UnlockExposedCells();
        }

        private void AssignPlayerCell()
        {
            _data.CurrentCell = TryGetCellAtLocation(_data.gameObject.transform.position + new Vector3(0, 0.5f, 0));
        }

        private void AssignCells(GridElement gridElement)
        {
            //var cells = new List<Cell>();
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
            }
        }

        public Cell TryGetCellAtLocation(Vector3 location) => _model.TryGetCellAtLocation(location);

        public GridElement GetElementFromCell(Cell cell)
        {
            _cellToElementMap.TryGetValue(cell, out var element);
            return element;
        }

        public void BounceCell(Cell cell)
        {
            List<Cell> cellRow = new List<Cell>();

            var topCell = cell;
            cellRow.Add(topCell);
            while (_model.TryGetCellAtLocation(topCell.GridPosition - new Vector3(0, 0.5f, 0)) != null)
            {
                topCell = _model.TryGetCellAtLocation(topCell.GridPosition - new Vector3(0, 0.5f, 0));
                cellRow.Add(topCell);
            }

            foreach (var cellInRow in cellRow)
            {
                _view.BounceGridElement(_cellToElementMap[cellInRow]);
            }
        }

        public void RemoveElement(GridElement gridElement)
        {
            for (int x = 0; x < gridElement.GridSize.x; x++)
            {
                for (int z = 0; z < gridElement.GridSize.z; z++)
                {
                    var cellUnderneath = _model.TryGetCellAtLocation(
                        gridElement.transform.position + new Vector3(x, 0.5f, z));
                    
                    if (cellUnderneath != null)
                    {
                        cellUnderneath.IsAccessible = true;
                    }
                }
            }
            
            _view.GridElements.Remove(gridElement);
            for (int x = 0; x < gridElement.GridSize.x; x++)
            {
                for (float y = 0; y < gridElement.GridSize.y; y += 0.5f)
                {
                    for (int z = 0; z < gridElement.GridSize.z; z++)
                    {
                        var key = gridElement.transform.position + new Vector3(x, y + 1, z);
                        if (!_model.GridLocationToCell.TryGetValue(key, out var value))
                        {
                            return;
                        }
                        _cellToElementMap.Remove(value);
                        _model.GridLocationToCell.Remove(key);
                    }
                }
            }
        }
    }
}
