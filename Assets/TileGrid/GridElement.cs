using System;
using UnityEngine;

namespace LevelStructure
{
    /// <summary>
    /// Foundational Component for communicating with the Beaked! tile grid.
    /// </summary>
    [SelectionBase]
    public class GridElement : MonoBehaviour
    {
        [field: SerializeField] public Vector3 GridSize { get; private set; } = Vector3.one;
        [field: SerializeField] public bool IsSolid { get; private set; } = true;
		[field: SerializeField] public bool IsClimbable { get; private set; } = true;
        
        public Vector3 GridOffset => new ((GridSize.x - 1) / 2, (GridSize.y - 1) / 2, (GridSize.z - 1) / 2);

        public Action<GridElement> OnDestroyOrDisable;

        private void OnDestroy()
        {
            OnDestroyOrDisable?.Invoke(this);
        }
    }
}
