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

        public Action<GridElement> OnDestroyOrDisable;

        private void OnDestroy()
        {
            OnDestroyOrDisable?.Invoke(this);
        }
    }
}
