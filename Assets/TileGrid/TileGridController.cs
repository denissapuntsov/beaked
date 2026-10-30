using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridController : MonoBehaviour
    {
        private TileGridModel _model;
        private TileGridView _view;
        private PlayerData _data;
        private FakePlayer _fakePlayer;
        public Dictionary<Cell, GridElement> CellToElementMap { get; private set; } = new();

        private void Awake()
        {
            _data = FindAnyObjectByType<PlayerData>();
            _fakePlayer = FindAnyObjectByType<FakePlayer>();
        }

        public void Setup()
        {
            DOTween.Init();
            //DOTween.KillAll();
            SetupGrid();
            _fakePlayer?.gameObject.SetActive(false);
            AssignPlayerCell();
            AssignSmartMoverCells();
        }

        private void AssignSmartMoverCells()
        {
            var smartMovers = FindObjectsByType<SmartMover>();
            foreach (var smartMover in smartMovers)
            {
                var moverCell = TryGetCellAtLocation(smartMover.transform.position);
                if (moverCell == null)
                {
                    Debug.Log($"{smartMover.name} has no cell at position {smartMover.transform.position}");
                }
                smartMover.currentCell = moverCell;
            }
        }

        public void Animate(Action onComplete)
        {
            AnimateWorldUp(onComplete);
        }

        public void AnimateWorldUp(Action onComplete)
        {
            var world = FindAnyObjectByType<World>();
            if (world == null)
            {
                AnimateFakePlayerUp(onComplete);
            }
            world.Setup(() => AnimateFakePlayerUp(onComplete));
        }

        private void AnimateFakePlayerUp(Action onComplete)
        {
            _fakePlayer.gameObject.SetActive(true);
            _fakePlayer.onSetupAnimationEnd.AddListener(() => onComplete?.Invoke());
            _fakePlayer.GetComponent<AnimationControls>().PlayAnimation("Setup");
        }

        public void AnimateWorldDown(Action onComplete)
        {
            var world =  FindAnyObjectByType<World>();
            if (world == null)
            {
                AnimateFakePlayerDown(onComplete);
            }
            world.Breakdown(() => AnimateFakePlayerDown(onComplete));
        }

        private void AnimateFakePlayerDown(Action onComplete)
        {
            _fakePlayer.gameObject.SetActive(true);
            _fakePlayer.onBreakdownAnimationEnd.AddListener(() => onComplete?.Invoke());
            _fakePlayer.GetComponent<AnimationControls>().PlayAnimation("Fall");
        }

        private void SetupGrid()
        {
            CellToElementMap = new Dictionary<Cell, GridElement>();
            _model = new TileGridModel();
            _view = new TileGridView(_data);

            var gridElements = FindObjectsByType<GridElement>(
                FindObjectsInactive.Include);

            foreach (var gridElement in gridElements)
            {
                AddElement(gridElement);
                gridElement.OnChange = Rescan;
            }
        }

        private void Rescan(GridElement gridElement)
        {
            RemoveElement(gridElement);
            AddElement(gridElement);
        }

        public void AssignPlayerCell()
        {
            if (!_data) return;
            var playerCell = TryGetCellAtLocation(_data.gameObject.transform.position);
            Debug.Log(playerCell.GridPosition);
            _data.CurrentCell = playerCell;
        }

        public Cell TryGetCellAtLocation(Vector3 location)
        {
            var roundedLocation = new Vector3 (
                (float)Math.Round(location.x, 2),
                (float)Math.Round(location.y, 2),
                (float)Math.Round(location.z, 2));
            return _model.TryGetCellAtLocation(roundedLocation);
        }


        public GridElement GetElementFromCell(Cell cell)
        {
            if (cell == null) return null;
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
                cell.IsAccessible = true;
                
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
            gridElement.cells = new List<Cell>();
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
                            IsAccessible = gridElement.IsClimbable,
                            IsSolid = gridElement.IsSolid
                        };
                        
                        cells.Add(newCell);
                        gridElement.cells.Add(newCell);

                        var cellAbove = _model.TryGetCellAtLocation(newCell.GridPosition + new Vector3(0, 0.5f, 0));
                        if (cellAbove != null && gridElement.IsClimbable)
                        {
                            newCell.IsAccessible = !cellAbove.IsSolid;
                        }
                        
                        var cellBelow = _model.TryGetCellAtLocation(newCell.GridPosition - new Vector3(0, 0.5f, 0));
                        if (cellBelow != null)
                        {
                            cellBelow.IsAccessible = !newCell.IsSolid;
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
