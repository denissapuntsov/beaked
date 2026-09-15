using System.Collections.Generic;
using LevelStructure;
using UnityEngine;

public class CarControls : ChainableAction
{
    [SerializeField] private bool isByGate;
    public List<GridElement> tilesToCheckForward, tilesToCheckBackward;
    private GateControls _gateControls;

    [SerializeField] private Mover forward, backward;
    
    private PlayerData _data;
    
    private void Start()
    {
        _data = FindAnyObjectByType<PlayerData>();
    }

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
        OnComplete += action => isByGate = true;
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
        return _data?.CurrentCell.GridPosition == cellPosition;
    }
}
