using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;
using GridElement = LevelStructure.GridElement;

public class Mover : ChainableAction
{
    [SerializeField] private float speed = 1;
    public List<Vector3> path = new();
    private TileGridController _controller;
    private GridElement _gridElement;
    
    private int _currentPathIndex;
    /*public Action<IChainableAction> OnComplete { get; set; }
    
    public Dictionary<IChainableAction, bool> ActionsCompleteMap { get; set; } = new();*/

    [SerializeField] private UnityEvent<ChainableAction> onCompletePath;
    
    private void Start()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _gridElement = GetComponent<GridElement>();
    }

    // TODO: falls apart due to rounding errors, fix
    /*private bool CheckPathForValidity()
    {
        foreach (var location in path)
        {
            var cell = _controller.TryGetCellAtLocation(location - _gridElement.GridOffset);
            if (cell != null) continue;
            Debug.LogError($"No cell found at {location}!");
            return false;
        }

        return true;
    }*/

    public void Move(ChainableAction source)
    {
        source.AddAction(this);
        
        // TODO: see above
        /*if (!CheckPathForValidity())
        {
            Complete();
            return;
        }*/
        
        _controller.RemoveElement(_gridElement);
        MoveTo(_currentPathIndex);
    }

    private void MoveTo(int index)
    {
        var distance = Vector3.Distance(transform.position, path[index]);
        
        Tween move;
        if (Mathf.Abs(path[index].y - transform.position.y) > 0.1f)
        {
            move = transform.DOJump(path[index], 0.5f, 1, 1 / speed);
        }
        else
        {
            move = transform.DOMove(path[index], distance / speed);
        }

        move.SetEase(Ease.Linear);
        move.onComplete = CheckForPathComplete;
    }
    
    private void CheckForPathComplete()
    {
        if (_currentPathIndex == path.Count - 1)
        {
            _controller.AddElement(_gridElement);
            onCompletePath?.Invoke(this);
            Complete();
            return;
        }

        _currentPathIndex++;
        MoveTo(_currentPathIndex);
    }
    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine((i == 0 ? transform.position : path[i-1]) + new Vector3(0, 0.2f, 0), path[i] + new Vector3(0, 0.2f, 0));
            
            Gizmos.color = i < path.Count - 1 ? Color.yellow : Color.red;
            Gizmos.DrawCube(path[i] + new Vector3(0, 0.2f, 0), new Vector3(0.6f, 0.15f, 0.6f * GetComponent<GridElement>().GridSize.z));
        }
    }
}
