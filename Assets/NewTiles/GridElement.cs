using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace NewTiles
{
    [SelectionBase]
    public class GridElement : MonoBehaviour
    {
        public Transform pivot;
        
        public Vector3 GridPosition => transform.localPosition;
        public Vector3/*Int*/ gridSize = Vector3/*Int*/.one;
        public bool isSolid = true;
        
        private Tweener _bounce;

        public void OnValidate()
        {
            if (pivot == null) return;
            pivot.localScale = gridSize;
        }
        
        public void Bounce() => Bounce(1f);

        public void Bounce(float strength)
        {
            var gridPosition = GridPosition;
            _bounce = transform
                .DOPunchPosition(new Vector3(0, -0.2f * strength, 0), 0.5f, 2, 2)
                .OnComplete(() => transform.localPosition = gridPosition);
        }
    }
}
