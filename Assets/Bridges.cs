using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using UnityEngine;

public class Bridges : ChainableAction
{
    private Tweener _move;

    [SerializeField] private List<Vector3> directions;
    [SerializeField] private int startingDirectionIndex;
    private int _positionIndex;

    private TileGridController _controller;
    
    private GridElement[] _elements = {};

    private void Awake()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _elements = GetComponentsInChildren<GridElement>();
    }

    private void Start()
    {
        _positionIndex = startingDirectionIndex;
        _move.Kill();
    }

    public void Switch(ChainableAction parent)
    {
        foreach (var element in _elements)
        {
            _controller.RemoveElement(element);
        }
        parent.AddAction(this);
        _move = transform
            .DOMove(directions[_positionIndex], 1f)
            .SetSpeedBased()
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .OnComplete(Finish);
    }

    private void Finish()
    {
        foreach (var element in _elements)
        {
            _controller.AddElement(element);
        }
        
        if (_positionIndex >= directions.Count - 1)
        {
            _positionIndex = 0;
        }
        else
        {
            _positionIndex++;
        }
        
        Complete();
    }
}
