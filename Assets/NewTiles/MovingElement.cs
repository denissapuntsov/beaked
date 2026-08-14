using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Events;

namespace NewTiles
{
    [RequireComponent(typeof(TileAttachment))]
    public class MovingElement : MonoBehaviour
    {
        private TileAttachment _attachment;
        public List<PathElement> path;

        public float speed = 2f;
        private Tween _move;
        private Sequence _turn;
        private int _positionOnPath = 0;
        
        public UnityEvent onPathEndReached, onPathObstructed;

        private void Start()
        {
            _attachment = GetComponent<TileAttachment>();
            Move();
        }

        public void Move()
        {
            MoveToPosition(_positionOnPath + 1);
        }

        public void Stop()
        {
            
        }

        private void MoveToPosition(int index)
        {
            if (index >= path.Count)
            {
                Debug.Log("Reached end of path");
                onPathEndReached?.Invoke();
                return;
            }
            Turn(path[index].orientation);
            TileGrid.instance.GetCell(path[index].gridElement.GridPosition + new Vector3(0, 1.5f, 0), out var nextCell);
            if (nextCell is { isAccessible: true, isSolid: true })
            {
                MoveTo(nextCell);
            }
            else
            {
                Debug.Log("Path obstructed");
                onPathObstructed?.Invoke();
            }
        }
        
        private void MoveTo(Cell cell)
        {
            _move = gameObject.transform
                .DOMove(cell.worldPosition, 1 / speed)
                .SetEase(Ease.Linear)
                .SetAutoKill()
                .OnComplete(() =>
                {
                    _positionOnPath++;
                    Move();
                });
        }

        private void Turn(Orientation orientation)
        {
            if (orientation == Orientation.Forward) return;
            var angle = orientation switch
            {
                Orientation.Left => -90,
                Orientation.Right => 90,
                _ => 0
            };
            _turn = DOTween.Sequence();
            _turn.AppendInterval(1 / speed / 2);
            _turn.Append(transform.DOBlendableRotateBy(new Vector3(0, angle, 0), 1 / speed));
            _turn.Restart();
        }
    }

    [System.Serializable]
    public class PathElement
    {
        public GridElement gridElement;
        public Orientation orientation;
    }
    
    public enum Orientation
    {
        Forward,
        Left,
        Right
    }
}
