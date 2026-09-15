using System;
using System.Collections.Generic;
using LevelStructure;
using UnityEngine;

public class CheckForListenersState : GameState
{
    public Listener FoundListener { get; private set; }
    private PlayerData _data;
    private TileGridController _controller;
    
    public CheckForListenersState(PlayerData data, TileGridController controller) : base()
    {
        _data = data;
        _controller = controller;
    }

    public override void Enter()
    {
        Check();
    }
    
    private void Check()
    {
        var cellToCheck = GetCellInFrontOfPlayer();
        if (cellToCheck == null)
        { 
            Complete();
            return;
        }
        FoundListener = FindListener(cellToCheck);
                
        Complete();
    }

    
    private Cell GetCellInFrontOfPlayer()
    {
        var front = _data.orientation switch
        {
            Orientation.North => new Vector3(0, 0, 1),
            Orientation.East => new Vector3(1, 0, 0),
            Orientation.South => new Vector3(0, 0, -1),
            Orientation.West => new Vector3(-1, 0, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
        var foundCell = _controller.TryGetCellAtLocation(_data.CurrentCell.GridPosition + front);
        return foundCell;
    }

    private Listener FindListener(Cell cell)
    {
        var foundCell = _controller.TryGetCellAtLocation(cell.GridPosition + new Vector3(0, 0.5f, 0));
        var element = _controller.GetElementFromCell(foundCell);
        if (foundCell == null || !element) return null;
                
        var listener = element.GetComponent<Listener>();
        return listener ? listener : null;
    }

    public override void Exit()
    {
        FoundListener = null;
    }
}
