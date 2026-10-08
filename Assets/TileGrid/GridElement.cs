using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace LevelStructure
{
    /// <summary>
    /// Foundational Component for communicating with the Beaked! tile grid.
    /// </summary>
    [SelectionBase]
    public class GridElement : MonoBehaviour
    {
        // width, height, length
        [field: SerializeField] public Vector3 GridSize { get; private set; } = Vector3.one;
        [field: SerializeField] public bool IsSolid { get; private set; } = true;
        [field: SerializeField] public bool IsClimbable { get; private set; } = true;

        public ElementType elementType;
        
        public List<Cell> cells;
        public Vector3 GridOffset => (GridSize.x - 1) / 2 * transform.right + (GridSize.z - 1) / 2 * transform.forward;

        public Action<GridElement> OnChange;
        public Action<GridElement> OnDestroyOrDisable;

        private void OnDisable()
        {
            OnDestroyOrDisable?.Invoke(this);
        }

        public void SetSolid(bool isSolid)
        {
            IsSolid = isSolid;
            foreach (var cell in cells)
            {
                cell.IsSolid = isSolid;
            }

            OnChange?.Invoke(this);
        }
    }
}

public enum ElementType
{
    Block,
    Decoration,
    Person,
    Player
}
