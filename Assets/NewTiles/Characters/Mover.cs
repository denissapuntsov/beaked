/*
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Detector))]
public class Mover : MonoBehaviour
{
    private Detector _detector;
    private Tweener _move;
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

    public void MoveToTile(Tile tile, Action onPathComplete)
    {
        var path = AStar.instance.CalculatePath(_detector.OccupiedTile, tile, out Tile reachableTile);
        StartPath(path, reachableTile, () => onPathComplete?.Invoke());
    }
}
*/
