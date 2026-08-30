using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        
        [HideInInspector] public List<Cell> cells;
        public Vector3 GridOffset => (GridSize.x - 1) / 2 * transform.right + (GridSize.z - 1) / 2 * transform.forward;

        public Action<GridElement> OnDestroyOrDisable;
        
        private void OnDestroy()
        {
            OnDestroyOrDisable?.Invoke(this);
        }
    }
}
