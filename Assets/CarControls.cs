using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Sequence = DG.Tweening.Sequence;

public class CarControls : ChainableAction
{
    [SerializeField] private List<CarPath> paths = new List<CarPath>();
    [SerializeField] private float speed = 2;
    
    private Dictionary<string, CarPath> _paths = new ();

    private CarPath _currentPath;
    private TileGridController _controller;

    private Tweener _move;

    private int _currentNodeIndex;

    private Cell _rear, _front;

    private GridElement _element;

    private void Start()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _element = GetComponent<GridElement>();
        if (paths.Count > 0)
        {
            _currentPath = paths[0];
        }    

        foreach (CarPath path in paths)
        {
            _paths.Add(path.key, path);
        }

        ReassignFrontAndBack();
    }

    private void ReassignFrontAndBack()
    {
        _front = _element.cells[0];
        _rear = _element.cells[1];
        if (_element.cells[1].GridPosition.x > _element.cells[0].GridPosition.x ||
            _element.cells[1].GridPosition.z > _element.cells[0].GridPosition.z)
        {
            _front = _element.cells[1];
            _rear = _element.cells[0];
        }
    }

    public void SetPath(string key)
    {
        _paths.TryGetValue(key, out _currentPath);
        _currentNodeIndex = 0;
    }

    public void Move(ChainableAction parent)
    {
        parent.AddAction(this);
        if (!IsPathFree())
        {
            Complete();
        }
        else
        {
            _currentPath.onStartPath?.Invoke(this);
            MoveToNextNodeInPath();
            Complete();
        }
    }

    private void MoveToNextNodeInPath()
    {
        _controller.RemoveElement(_element);
        Debug.Log("moving");
        int index = _currentNodeIndex;
        if (index == _currentPath.directions.Count)
        {
            _controller.AddElement(_element);
            ReassignFrontAndBack();
            _currentPath.onFinishPath?.Invoke(this);
            Complete();
            return;
        }
        _currentNodeIndex++;
        
        var nextDirection = new Vector3Int((int)_currentPath.directions[index].x, 0, (int)_currentPath.directions[index].y);
        
        Debug.Log(nextDirection);
        _move = transform
            .DOMove(nextDirection, speed)
            .SetRelative()
            .SetSpeedBased()
            .SetAutoKill()
            .SetEase(Ease.Linear)
            .OnComplete(MoveToNextNodeInPath);
    }

    private bool IsPathFree()
    {
        var lastCellCheckedCoordinates = _front.GridPosition;
        foreach (Vector2 node in _currentPath.directions)
        {
            var nextCellOffset = new Vector3Int((int)node.x, 0, (int)node.y);
            var zeroCell = _front;
            if (nextCellOffset.x < 0 || nextCellOffset.z < 0)
            {
                zeroCell = _rear;
            }

            if (zeroCell == _rear && lastCellCheckedCoordinates == _front.GridPosition)
            {
                lastCellCheckedCoordinates = _rear.GridPosition;
            }
            
            var nextCell = _controller.TryGetCellAtLocation(nextCellOffset + lastCellCheckedCoordinates);
            
            if (nextCell != null)
            {
                Debug.Log($"path is blocked at {nextCellOffset + lastCellCheckedCoordinates}! by {_controller.GetElementFromCell(nextCell)}");
                return false;
            }

            lastCellCheckedCoordinates += nextCellOffset;
        }

        return true;
    }
}

[Serializable]
public class CarPath
{
    public string key;
    public List<Vector2> directions;
    public UnityEvent<ChainableAction> onStartPath, onFinishPath;
}