using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

public class Bridges : ChainableAction
{
    private Tweener _move;

    [SerializeField] private List<MoveCommand> commands;
    [SerializeField] private int startingCommandIndex;
    private int _commandIndex;

    private bool _carriesPlayer;

    private PlayerData _data;

    private TileGridController _controller;
    
    private GridElement[] _elements = {};

    private void Awake()
    {
        _data = FindAnyObjectByType<PlayerData>();
        _controller = FindAnyObjectByType<TileGridController>();
        _elements = GetComponentsInChildren<GridElement>();
    }

    private void Start()
    {
        _commandIndex = startingCommandIndex;
        _move.Kill();
    }

    public void Switch(ChainableAction parent)
    {
        foreach (var element in _elements)
        {
            foreach (var cell in element.cells)
            {
                if (_data.CurrentCell.GridPosition == cell.GridPosition)
                {
                    //_controller.RemoveElement(_data.gridElement);
                    _data.transform.SetParent(transform);
                    _carriesPlayer = true;
                }
            }
        }
        
        foreach (var element in GetComponentsInChildren<GridElement>())
        {
            _controller.RemoveElement(element);
        }
        parent.AddAction(this);
        commands[_commandIndex].onStart?.Invoke(this);
        
        _move = transform
            .DOMove(commands[_commandIndex].direction, commands[_commandIndex].speed)
            .SetSpeedBased()
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .OnComplete(Finish);
    }

    private void Finish()
    {
        foreach (var element in GetComponentsInChildren<GridElement>())
        {
            _controller.AddElement(element);
        }
        
        if (_carriesPlayer)
        {
            _data.transform.SetParent(transform.root);
            //_controller.AddElement(_data.gridElement);
            _controller.AssignPlayerCell();
            _carriesPlayer = false;
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
public class MoveCommand
{
    public Vector3 direction;
    public float speed = 2;
    public UnityEvent<ChainableAction> onStart, onFinish;
}
