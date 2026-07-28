using System.Collections.Generic;
using UnityEngine;

namespace NewTiles
{
    public class GridElement : MonoBehaviour
    {
        [HideInInspector] public Vector3 gridPosition;
        public Vector3/*Int*/ gridSize = Vector3/*Int*/.one;
    }
}
