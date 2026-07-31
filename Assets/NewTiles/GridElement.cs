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

        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            var yPos = transform.localPosition.y;
            var xPos = transform.position.x;
            var zPos = transform.position.z;
            transform.localPosition = new Vector3(transform.localPosition.x, yPos + 20 + zPos * 15 + xPos * 10, transform.localPosition.z);
            transform
                .DOLocalMoveY(yPos, 40f)
                .SetSpeedBased()
                .SetEase(Ease.OutBack, 0.2f);
        }
        
        public void Bounce() => Bounce(1f);

        public void Bounce(float strength)
        {
            var gridPosition = GridPosition;
            _bounce.Kill();
            _bounce = transform
                .DOPunchPosition(new Vector3(0, -0.2f * strength, 0), 0.5f, 2, 2)
                .OnComplete(() => transform.localPosition = gridPosition);
        }
    }
}
