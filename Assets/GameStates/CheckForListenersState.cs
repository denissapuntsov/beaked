using System.Collections.Generic;
using NewTiles;
using UnityEngine;

public class CheckForListenersState : GameState
{
    public List<Listener> listenersFound = new();
    
    public CheckForListenersState() : base()
    {
        
    }

    public override void Enter()
    {
        var surroundingCells = GetSurroundingCells();
        listenersFound = FindListeners(surroundingCells);

        Complete();
    }

    private List<NewTiles.Cell> GetSurroundingCells()
    {
        var cellsAroundPlayer = new List<NewTiles.Cell>();

        var offsets = new List<Vector2>
        {
            new (0, 1),
            new (0, -1),
            new (-1, 0),
            new (1, 0)
        };

        for (var index = 0; index < offsets.Count; index++)
        {
            var offset = offsets[index];
            NewTiles.Cell newCell;
            TileGrid.instance.GetCellAt(
                new Vector3(
                    PlayerData.Instance.CurrentCell.gridPosition.x + offset.x,
                    PlayerData.Instance.CurrentCell.gridPosition.y,
                    PlayerData.Instance.CurrentCell.gridPosition.z + offset.y),
                out newCell);
            if (newCell != null)
            {
                cellsAroundPlayer.Add(newCell);
            }
        }

        return cellsAroundPlayer;
    }

    private List<Listener> FindListeners(List<NewTiles.Cell> cellsAroundPlayer)
    {
        var listeners = new List<Listener>();

        foreach (var cell in cellsAroundPlayer)
        {
            TileGrid.instance.GetCellAt(cell.gridPosition + new Vector3(0, 0.5f, 0), out var foundCell);
            if (foundCell == null || !foundCell.occupyingElement) continue;
            var listener = foundCell.occupyingElement.gameObject.GetComponent<Listener>();
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
