using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

public class SmartMover : ChainableAction
{
    [SerializeField] private List<TilePath> paths = new List<TilePath>();
    
    private Dictionary<string, List<Vector2>> _paths = new ();

    private List<Vector2> _currentPath;
    private int _currentNodeIndex;
    public Cell currentCell;
    private Tween _move, _jump;
    
    [SerializeField] private float speed = 2;
    
    private TileGridController _controller;
    private GridElement _gridElement;

    public UnityEvent<ChainableAction> onStartPath, onCompletePath;
    
    private void Start()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _gridElement = GetComponent<GridElement>();
        
        InitializePaths();
    }

    private void InitializePaths()
    {
        _paths.Clear();
        foreach (var path in paths)
        {
            _paths.Add(path.key, path.directions);
        }
    }

    public void Move(ChainableAction parent)
    {
        parent.AddAction(this);
        if (_currentPath?.Count == 0)
        {
            Complete();
            return;
        }
        Debug.Log($"moving on path {_currentPath}");
        
        _controller.RemoveElement(_gridElement);
        onStartPath?.Invoke(this);
        MoveToNextNodeInPath();
    }

    public void SelectPath(string key)
    {
        _paths.TryGetValue(key, out _currentPath);
        _currentNodeIndex = 0;
    }    
    
    public void Turn()
    {
        
    }

    private void MoveToNextNodeInPath()
    {
        int index = _currentNodeIndex;
        if (index == _currentPath.Count)
        {
            _controller.AddElement(_gridElement);
            Complete();
            return;
        }
        _currentNodeIndex++;

        Cell nextCell;
        var nextCellDirection = new Vector3(_currentPath[index].x, 0,  _currentPath[index].y);
        foreach (var f in new List<float> { 0f, 0.5f, -0.5f })
        {
            nextCell = _controller.TryGetCellAtLocation(nextCellDirection + currentCell.GridPosition + new Vector3(0, f, 0));
            //TileGrid.instance.GetCellAt(nextTileCoordinates + _data.CurrentCell.gridPosition + new Vector3(0, f, 0), out var nextCell);
            if (nextCell is not { IsAccessible: true, IsSolid: true }) continue;
            if (f != 0)
            {
                MoveToCell(nextCell);
                return;
            }
            JumpToCell(nextCell);
            return;
        }
        _controller.AddElement(_gridElement);
        Complete();

    }

    private void JumpToCell(Cell cell)
    {
        _jump?.Kill();
        cell.IsAccessible = false;
        _jump = transform
            .DOJump(cell.GridPosition, 0.4f, 1, speed / 4)
            .SetSpeedBased()
            .SetAutoKill()
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                currentCell.IsAccessible = true;
                currentCell = cell;
                MoveToNextNodeInPath();
            });

    }

    private void MoveToCell(Cell cell)
    {
        _move?.Kill();
        
        //_controller.RemoveElement(_data.gridElement);
        cell.IsAccessible = false;
        _move = transform
            .DOMove(cell.GridPosition, speed)
            .SetSpeedBased()
            .SetAutoKill()
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                currentCell.IsAccessible = true;
                currentCell = cell;
                MoveToNextNodeInPath();
            });
    }

    protected override void Complete()
    {
        base.Complete();
        onCompletePath?.Invoke(this);
    }
    
}

[Serializable] 
public class TilePath
{
    public string key;
    // x is x, y is z
    public List<Vector2> directions;
}
