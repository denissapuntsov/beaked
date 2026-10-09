using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace LevelStructure
{
    public class TileGridView
    {
        public List<GridElement> GridElements = new();
        
        private GridElement _zeroElement;
        
        private Dictionary<GridElement, bool> _blocksCompleteMap = new();
        private Dictionary<GridElement, bool> _decorationsCompleteMap = new();
        private Dictionary<GridElement, bool> _peopleCompleteMap = new();
        
        private List<Dictionary<GridElement, bool>> _groups = new();

        private Action _onGridAnimationComplete;

        private Dictionary<GridElement, Vector3> _startingPositions;
        private Dictionary<GridElement, float> _offsets;
        
        private PlayerData _playerData;

        public TileGridView(PlayerData playerData)
        {
            _playerData = playerData;
        }

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
            SetupGrid(onComplete);
        }

        private void SetupGrid(Action onComplete)
        {
            _groups = new()
            {
                _blocksCompleteMap,
                _decorationsCompleteMap,
                _peopleCompleteMap
            };
            _onGridAnimationComplete = onComplete;
            
            _startingPositions = new();
            _offsets = new();

            _zeroElement = GridElements[0];
            
            foreach (var element in GridElements)
            {
                if (element.transform.position.x > _zeroElement.transform.position.x ||
                    element.transform.position.y > _zeroElement.transform.position.y ||
                    element.transform.position.z > _zeroElement.transform.position.z)
                {
                    continue;
                }

                _zeroElement = element;
            }

            foreach (var element in GridElements)
            {
                SetupElement(element);
            }
            
            DropNextGroup();
        }

        private void DropNextGroup()
        {
            Debug.Log(_groups.Count);
            if (_groups.Count == 0)
            {
                _onGridAnimationComplete?.Invoke();
                return;
            }
            DropGroup(_groups[0]);
            _groups.RemoveAt(0);
        }

        private void DropGroup(Dictionary<GridElement, bool> map)
        {
            Debug.Log("Dropping group");
            foreach (var pair in map)
            {
                var element = pair.Key;
                element.transform
                    .DOBlendableMoveBy(
                        new Vector3(0, -_offsets[element], 0),
                        50f)
                    .SetSpeedBased(true)
                    .SetEase(Ease.OutBack, 0.5f)
                    .OnComplete(() =>
                    {
                        element.transform.position = _startingPositions[element];
                        map[element] = true;
                        if (!map.Values.Contains(false))
                        {
                            DropNextGroup();
                        }
                    });
            }
        }

        private void SetupElement(GridElement element)
        {
            switch (element.elementType)
            {
                case ElementType.Block:
                    _blocksCompleteMap.Add(element, false);
                    break;
                case ElementType.Decoration:
                    _decorationsCompleteMap.Add(element, false);
                    break;
                case ElementType.Person:
                    _peopleCompleteMap.Add(element, false);
                    break;
            }
            
            //_elementsAnimationCompleteMap.Add(element, false);
            var startingPosition = element.transform.position;
            float distance = Vector3.Distance(startingPosition, _zeroElement.transform.position);
            _startingPositions.Add(element, startingPosition);
            var offset =
                10 * distance;
            _offsets.Add(element, offset);
            element.transform.position += new Vector3(0, offset, 0);
        }

        /*private void DropElement(GridElement element)
        {
            element.transform
                .DOBlendableMoveBy(
                    new Vector3(0, -_offsets[element], 0),
                    50f)
                .SetSpeedBased(true)
                .SetEase(Ease.OutBack, 0.2f)
                .OnComplete(() =>
                {
                    element.transform.position = _startingPositions[element];
                    SetElementAnimationComplete(element);
                });
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
        }*/
    }
}