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
                FindObjectsSortMode.InstanceID);

            foreach (var gridElement in gridElements)
            {
                AddElement(gridElement);
            }
        }

        private void AssignPlayerCell()
        {
            if (!_data) return;
            _data.CurrentCell = TryGetCellAtLocation(_data.gameObject.transform.position);
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
            foreach (var cell in gridElement.cells)
            {
                var cellUnderneath = _model.TryGetCellAtLocation(cell.GridPosition - new Vector3(0, 0.5f, 0));
                    
                if (cellUnderneath != null)
                {
                    cellUnderneath.IsAccessible = true;
                }
                
                CellToElementMap.Remove(cell);
                _model.GridLocationToCell.Remove(cell.GridPosition);
            }
            
            _view.GridElements.Remove(gridElement);
        }

        public void AddElement(GridElement gridElement)
        {
            gridElement.cells.Clear();
            _view.GridElements.Add(gridElement);
            Vector3 startingPosition = gridElement.transform.position - gridElement.GridOffset;
            
            List<Cell> cells = new List<Cell>();
            
            for (var x = 0f; x < gridElement.GridSize.x; x++)
            {
                for (var y = gridElement.GridSize.y; y > 0; y -= 0.5f)
                {
                    for (var z = 0f; z < gridElement.GridSize.z; z++)
                    {
                        var gridPosition = 
                            startingPosition +
                            x * gridElement.transform.right +
                            y * gridElement.transform.up +
                            z * gridElement.transform.forward;
                        
                        var roundedPosition = new Vector3(
                            (float)Math.Round(gridPosition.x, 2),
                            (float)Math.Round(gridPosition.y, 2),
                            (float)Math.Round(gridPosition.z, 2));
                        
                        var newCell = new Cell
                        {
                            GridPosition = roundedPosition,
                            IsAccessible = gridElement.IsClimbable
                        };
                        cells.Add(newCell);
                        gridElement.cells.Add(newCell);

                        var cellAbove = _model.TryGetCellAtLocation(newCell.GridPosition + new Vector3(0, 0.5f, 0));
                        if (cellAbove != null && gridElement.IsClimbable)
                        {
                            newCell.IsAccessible = false;
                        }
                        
                        var cellBelow = _model.TryGetCellAtLocation(newCell.GridPosition - new Vector3(0, 0.5f, 0));
                        if (cellBelow != null)
                        {
                            cellBelow.IsAccessible = false;
                        }
                        
                        _model.GridLocationToCell.Add(newCell.GridPosition, newCell);
                        CellToElementMap.Add(newCell, gridElement);
                    }
                }
            }
            
            gridElement.OnDestroyOrDisable = RemoveElement;
        }
    }
}
