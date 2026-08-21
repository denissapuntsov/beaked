using UnityEngine;

namespace LevelStructure
{
    public class Cell
    {
        public Vector3 GridPosition;
        public Vector3 WorldPosition;

        //[HideInInspector] public GridElement occupyingElement;
        public bool IsAccessible;
        public bool IsSolid = true;

        //[HideInInspector] public UnityEvent onPlayerEnter = new UnityEvent();
        //[HideInInspector] public UnityEvent onPlayerExit = new UnityEvent();
    }
}
