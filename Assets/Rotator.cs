using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

public class Rotator : ChainableAction
{
    private Tweener _rotate;
    
    [SerializeField] private List<RotateCommand> commands;
    [SerializeField] private int startingCommandIndex;
    private int _commandIndex;
    
    private TileGridController _controller;
    
    private GridElement[] _elements = {};

    private void Awake()
    {
        _controller = FindAnyObjectByType<TileGridController>();
        _elements = GetComponentsInChildren<GridElement>();
    }
    
    public void Switch(ChainableAction parent)
    {
        foreach (var element in _elements)
        {
            _controller.RemoveElement(element);
        }
        parent.AddAction(this);
        commands[_commandIndex].onStart?.Invoke(this);
        
        _rotate = transform
            .DORotate(commands[_commandIndex].eulerAngles, commands[_commandIndex].speed * 30)
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
        
        commands[_commandIndex].onFinish?.Invoke(this);
        
        if (_commandIndex == commands.Count - 1)
        {
            _commandIndex = 0;
        }
        else
        {
            _commandIndex++;
        }
        
        Complete();
    }
}

[Serializable]
public class RotateCommand
{
    public Vector3 eulerAngles;
    public float speed = 2;
    public UnityEvent<ChainableAction> onStart, onFinish;
}
