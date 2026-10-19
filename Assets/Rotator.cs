using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rotator : ChainableAction
{
    private Tweener _rotate;
    
    [SerializeField] private List<MoveCommand> commands;
    [SerializeField] private int startingCommandIndex;
    private int _commandIndex;
}

[Serializable]
public class RotateCommand
{
    public Vector3 eulerAngles;
    public float speed = 2;
    
}
