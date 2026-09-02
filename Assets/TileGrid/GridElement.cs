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
        
        public List<Cell> cells;
        public Vector3 GridOffset => (GridSize.x - 1) / 2 * transform.right + (GridSize.z - 1) / 2 * transform.forward;

        public Action<GridElement> OnDestroyOrDisable;
        public Action<GridElement> OnChangeSize;
        
        private PopupManager _popupManager;

        private void Start()
        {
            _popupManager = FindAnyObjectByType<PopupManager>();
        }

        private void OnDisable()
        {
            OnDestroyOrDisable?.Invoke(this);
        }

        public void OpenPopup() => _popupManager.OpenPopup(this);
        public void ClosePopup() => _popupManager.ClosePopup(this);

        public void SetGridSize(int x = 0, float y = 0f, int z = 0)
        {
            var newX = x == 0 ? GridSize.x : x;
            var newY = y == 0f ? GridSize.y : y;
            var newZ = z == 0 ? GridSize.z : z;
            
            GridSize = new Vector3(newX, newY, newZ);
            OnChangeSize?.Invoke(this);
        }
    }
}
