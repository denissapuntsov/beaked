using System;
using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class CarControls : GridElementControls
{
    [SerializeField] private bool isByGate;
    public List<GridElement> tilesToCheckForward, tilesToCheckBackward;
    private GateControls _gateControls;
    [SerializeField] private Transform carBody;

    [SerializeField] private Mover forward, backward;

    public void Move(ChainableAction source)
    {
        if (!isByGate)
        {
            MoveForward(source);
        }
        else
        {
            MoveBackward(source);
        }
    }

    private void MoveForward(ChainableAction source)
    {
        foreach (var element in tilesToCheckForward)
        {
            foreach (var cell in element.cells)
            {
                if (CheckCell(cell.GridPosition))
                {
                    MoveBackward(source);
                    return;
                }
            }
        }

        source.AddAction(this);
        forward.Move(this);
        OnComplete += action =>
        {
            isByGate = true;
        };
    }

    public override void ReactToPeck(ChainableAction parent)
    {
        parent.AddAction(this);
        
        var turnDirection = Data.orientation switch
        {
            Orientation.North => new Vector3(0, 0, 1),
            Orientation.East => new Vector3(-1, 0, 0),
            Orientation.South => new Vector3(0, 0, -1),
            Orientation.West => new Vector3(1, 0, 0),
            _ => throw new ArgumentOutOfRangeException()
        };

        var turnVector = turnDirection * 5;

        carBody.DOBlendableLocalRotateBy(turnVector, 0.2f)
            .OnComplete(() => { carBody.DOBlendableLocalRotateBy(-turnVector, 0.4f); });
    }

    private void MoveBackward(ChainableAction source)
    {
        foreach (var element in tilesToCheckBackward)
        {
            foreach (var cell in element.cells)
            {
                if (CheckCell(cell.GridPosition))
                {
                    return;
                }
            }
        }
        
        source.AddAction(this);
        backward.Move(this);
        OnComplete += action => isByGate = false;
    }
    
    private bool CheckCell(Vector3 cellPosition)
    {
        return Data?.CurrentCell.GridPosition == cellPosition;
    }
}
