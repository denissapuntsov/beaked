using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Detector))]
public class PathFollower : MonoBehaviour
{
    private Detector _detector;
    private Tweener _move;
    
    [SerializeField] private List<Tile> sequence = new List<Tile>();
    public int speed = 5;
    
    private void Start()
    {
        _detector = GetComponent<Detector>();
    }

    #region Move to separate script to use on NPCs and player

    private void StartPath(List<Tile> path, Tile goal) => StartPath(path, goal, null);
    
    private void StartPath(List<Tile> path, Tile goal, Action onPathComplete)
    {
        if (path == null) return;
        _move?.Kill();
        path[1].IsBlocked = true;
        _move = transform
            .DOMove(path[1].WorldPosition, speed)
            .SetSpeedBased(true)
            .SetAutoKill()
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                if (_detector.OccupiedTile == goal)
                {
                    onPathComplete?.Invoke();
                    return;
                }
                var nextPath = AStar.instance.CalculatePath(_detector.OccupiedTile, goal, out Tile reachableGoal);
                StartPath(nextPath, reachableGoal, onPathComplete);
            });
    }

    #endregion
    public void TraverseSequence() => TraverseSequence(0);

    private void TraverseSequence(int sequenceIndex)
    {
        var path = AStar.instance.CalculatePath(_detector.OccupiedTile, sequence[sequenceIndex], out Tile reachableTile);
        StartPath(path, reachableTile, () =>
        {
            Debug.Log(sequence[sequenceIndex]);
            if (sequenceIndex == sequence.Count - 1)
            {
                return;
            }
            TraverseSequence(sequenceIndex + 1);
        });
    }
}
