using System;
using DG.Tweening;
using LevelStructure;
using UnityEngine;

public class Bridges : ChainableAction
{
    [SerializeField] private bool isUp = false;

    private Tweener _move;

    private TileGridController _controller;
    
    private GridElement[] _elements = new GridElement[]{};

    private void Awake()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _elements = GetComponentsInChildren<GridElement>();
    }

    private void Start()
    {
        
        _move.Kill();
    }

    public void Switch(ChainableAction parent)
    {
        var isUp = this.isUp;
        this.isUp = !this.isUp;
        foreach (var element in _elements)
        {
            _controller.RemoveElement(element);
        }
        parent.AddAction(this);
        _move = transform
            .DOMoveY(isUp ? -1 : 1, 1)
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
        Complete();
    }
}
