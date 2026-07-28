using System.Collections.Generic;
using UnityEngine;

namespace NewTiles
{
    public class GridElement : MonoBehaviour
    {
        public Vector3 gridPosition;
        public Vector3/*Int*/ gridSize = Vector3/*Int*/.one;
        public bool isSolid = true;
    }
}
