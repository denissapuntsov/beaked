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
        
        private PlayerData _playerData;
        private Action _onGridAnimationComplete;

        private Dictionary<GridElement, Vector3> _startingScale = new();
        private Dictionary<GridElement, bool> _completionMap = new();

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

        public void AnimateCamera(Action onComplete)
        {
            _onGridAnimationComplete = onComplete;
            _onGridAnimationComplete?.Invoke();
            var mainCamera = Camera.main;
            if (!mainCamera)
            {
                _onGridAnimationComplete?.Invoke();
                return;
            }
            mainCamera.transform.position += new Vector3(0, 50f, 0);
            mainCamera.transform
                .DOBlendableMoveBy(new Vector3(0, -50f, 0), 3f)
                .SetEase(Ease.OutBack, 0.5f)
                .OnComplete(() =>
                {
                    _onGridAnimationComplete?.Invoke();
                });
        }
        
    }
}