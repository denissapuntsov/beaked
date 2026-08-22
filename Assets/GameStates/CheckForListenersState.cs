using System.Collections.Generic;
using LevelStructure;
using UnityEngine;

public class CheckForListenersState : GameState
{
    public List<Listener> listenersFound = new();
    private PlayerData _data;
    private TileGridController _gridController;
    
    public CheckForListenersState(PlayerData data, TileGridController controller) : base()
    {
        _data = data;
        _gridController = controller;
    }

    public override void Enter()
    {
        var surroundingCells = GetSurroundingCells();
        listenersFound = FindListeners(surroundingCells);

        Complete();
    }

    private List<Cell> GetSurroundingCells()
    {
        var cellsAroundPlayer = new List<Cell>();

        var offsets = new List<Vector3>
        {
            new (0, 0, 1),
            new (0, 0, -1),
            new (-1, 0, 0),
            new (1, 0, 0)
        };

        for (var index = 0; index < offsets.Count; index++)
        {
            var offset = offsets[index];

            var newCell = _gridController.TryGetCellAtLocation(_data.CurrentCell.GridPosition + offset);
            if (newCell != null)
            {
                cellsAroundPlayer.Add(newCell);
            }
        }

        return cellsAroundPlayer;
    }

    private List<Listener> FindListeners(List<Cell> cellsAroundPlayer)
    {
        var listeners = new List<Listener>();

        foreach (var cell in cellsAroundPlayer)
        {
            var foundCell = _gridController.TryGetCellAtLocation(cell.GridPosition + new Vector3(0, 0.5f, 0));
            if (foundCell == null || !_gridController.GetElementFromCell(foundCell)) continue;
            var listener = _gridController.GetElementFromCell(foundCell).GetComponent<Listener>();
            if (listener)
            {
                listeners.Add(listener);
            }
        }

        return listeners;
    }

    public override void Exit()
    {
        listenersFound.Clear();
    }
}
