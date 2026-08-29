using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridController : MonoBehaviour
    {
        private TileGridModel _model;
        private TileGridView _view;
        private PlayerData _data;
        public Dictionary<Cell, GridElement> CellToElementMap { get; private set; } = new();

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
            CellToElementMap = new Dictionary<Cell, GridElement>();
            _model = new TileGridModel();
            _view = new TileGridView();

            var gridElements = FindObjectsByType<GridElement>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

            foreach (var gridElement in gridElements)
            {
                AddElement(gridElement);
            }
        }

        private void AssignPlayerCell()
        {
            if (!_data) return;
            _data.CurrentCell = TryGetCellAtLocation(_data.gameObject.transform.position + new Vector3(0, 0.5f, 0));
        }

        public Cell TryGetCellAtLocation(Vector3 location) => _model.TryGetCellAtLocation(location);

        public GridElement GetElementFromCell(Cell cell)
        {
            CellToElementMap.TryGetValue(cell, out var element);
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
                _view.BounceGridElement(CellToElementMap[cellInRow]);
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
                        var key = gridElement.transform.position + new Vector3(x, y + 1f, z);
                        _model.GridLocationToCell.TryGetValue(key, out var value);
                        if (value == null)
                        {
                            return;
                        }
                        CellToElementMap.Remove(value);
                        _model.GridLocationToCell.Remove(key);
                    }
                }
            }
        }

        public void AddElement(GridElement gridElement)
        {
            _view.GridElements.Add(gridElement);
            Vector3 startingPosition = gridElement.transform.position - gridElement.GridOffset;
            
            for (int x = 0; x < gridElement.GridSize.x; x++)
            {
                for (float y = gridElement.GridSize.y; y > 0; y -= 0.5f)
                {
                    for (int z = 0; z < gridElement.GridSize.z; z++)
                    {
                        var newCell = new Cell
                        {
                            GridPosition = startingPosition + new Vector3(x, y + 0.5f, z),
                        };
                        bool isCellAboveOccupied =
                            _model.TryGetCellAtLocation(newCell.GridPosition + new Vector3(0, 0.5f, 0)) != null;
                        newCell.IsAccessible =
                            y == gridElement.GridSize.y 
                                ? !isCellAboveOccupied && gridElement.IsClimbable 
                                : !isCellAboveOccupied;
                        
                        _model.GridLocationToCell.Add(newCell.GridPosition, newCell);
                        CellToElementMap.Add(newCell, gridElement);
                    }
                }
            }
            
            for (int x = 0; x < gridElement.GridSize.x; x++)
            {
                for (int z = 0; z < gridElement.GridSize.z; z++)
                {
                    var cellUnderneath = _model.TryGetCellAtLocation(
                        gridElement.transform.position + new Vector3(x, 0.5f, z));
                    
                    if (cellUnderneath != null)
                    {
                        cellUnderneath.IsAccessible = false;
                    }
                }
            }
            
            gridElement.OnDestroyOrDisable = RemoveElement;
        }
    }
}
