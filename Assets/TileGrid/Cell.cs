using System;
using UnityEngine;

namespace LevelStructure
{
    [Serializable]
    public class Cell
    {
        public Vector3 GridPosition;
        
        public bool IsAccessible;
        public bool IsSolid = true;

        //[HideInInspector] public UnityEvent onPlayerEnter = new UnityEvent();
        //[HideInInspector] public UnityEvent onPlayerExit = new UnityEvent();
    }
}
