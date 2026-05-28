using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Detector))] [RequireComponent(typeof(Listener))]
public class Person : MonoBehaviour
{
    public GameObject item;
    
    private Detector _detector;
    public Tile finalGoal;
    
    private Tweener _move;
    public int speed = 5;

    private void Start()
    {
        _detector = GetComponent<Detector>();
    }

    public void DropItem()
    {
        item.gameObject.SetActive(true);
    }

    public void RunAway()
    {
        var path = AStar.instance.CalculatePath(_detector.OccupiedTile, finalGoal, out Tile reachableGoal);
        StartPath(path, reachableGoal);
    }
    
    private void StartPath(List<Tile> path, Tile goal)
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
                    return;
                }
                var nextPath = AStar.instance.CalculatePath(_detector.OccupiedTile, goal, out Tile reachableGoal);
                StartPath(nextPath, reachableGoal);
            });
    }
}
