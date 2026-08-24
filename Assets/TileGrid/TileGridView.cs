using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridView
    {
        public List<GridElement> GridElements;

        private Dictionary<GridElement, bool> _elementsAnimationCompleteMap = new();

        private Action _onGridAnimationComplete;

        public void BounceGridElement(GridElement gridElement)
        {
            var startingPosition = gridElement.transform.position;
            gridElement.transform
                .DOPunchPosition(new Vector3(0, -0.2f, 0), 0.5f, 2, 2)
                .OnComplete(() =>
                {
                    gridElement.transform.position = startingPosition;
                });
        }

        public void AnimateGrid(Action onComplete)
        {
            _onGridAnimationComplete = onComplete;
            foreach (var element in GridElements)
            {
                _elementsAnimationCompleteMap.Add(element, false);
                var startingPosition = element.transform.position;
                var offset = 
                    20 + 
                    startingPosition.y + 
                    startingPosition.x * 10 + 
                    startingPosition.z * 15;
                element.transform.position += new Vector3(0, offset, 0);
                element.transform
                    .DOBlendableMoveBy(
                        new Vector3(0, -offset, 0),
                        40f)
                    .SetSpeedBased(true)
                    .SetEase(Ease.OutBack, 0.2f)
                    .OnComplete(() =>
                    {
                        element.transform.position = startingPosition;
                        SetElementAnimationComplete(element);
                    });
            }
        }

        private void SetElementAnimationComplete(GridElement element)
        {
            _elementsAnimationCompleteMap[element] = true;
            CheckAnimationsComplete();
        }

        private void CheckAnimationsComplete()
        {
            if (_elementsAnimationCompleteMap.Values.Contains(false)) return;
            _onGridAnimationComplete?.Invoke();
        }
    }
}