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
    [HideInInspector] public List<Vector3> path = new() { };
    [HideInInspector] public List<Vector3> offsets = new() { };
    private TileGridController _controller;
    private GridElement _gridElement;
    private PlayerData _data;
    public Color color;

    private int _currentPathIndex;

    public UnityEvent onStartPath;
    [SerializeField] private UnityEvent<ChainableAction> onCompletePath;

    private void Reset()
    {
        path.Add(transform.position);
    }

    private void Start()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _gridElement = GetComponent<GridElement>();
        _data = GetComponent<PlayerData>();
    }

    public void Move(ChainableAction source)
    {
        source?.AddAction(this);

        _controller.RemoveElement(_gridElement);
        onStartPath?.Invoke();
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
            move = transform.DOBlendableMoveBy(path[index], distance / speed);
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
            Gizmos.color = color;
            Gizmos.DrawLine((i == 0 ? transform.position : path[i-1]) + new Vector3(0, 0.2f, 0), path[i] + new Vector3(0, 0.2f, 0));
            
            Gizmos.color = i < path.Count - 1 ? color : Color.grey * color;
            Gizmos.DrawCube(path[i] + new Vector3(0, 0.2f, 0), new Vector3(0.6f, 0.15f, 0.6f * GetComponent<GridElement>().GridSize.z));
        }
    }
}
