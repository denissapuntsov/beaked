using System.Collections.Generic;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridModel
    {
        public Dictionary<Vector3, Cell> GridLocationToCell = new();

        public Cell TryGetCellAtLocation(Vector3 location)
        {
            GridLocationToCell.TryGetValue(location, out var cell);
            return cell;
        }
    }
}
