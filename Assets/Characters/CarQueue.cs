using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Speaker))]
public class CarQueue : MonoBehaviour
{
    public Tile entryTile, exitTile;
    public List<SpawnPoint> path;
    public List<GameObject> carVariants;
    private List<GameObject> _queue = new List<GameObject>();

    public UnityEvent onCarLeave;

    private Speaker _speaker;

    private bool _isInfinite = true;

    private void Start()
    {
        _speaker = GetComponent<Speaker>();
        Setup();
    }

    public void Setup()
    {
        foreach (SpawnPoint point in path)
        {
            var nextCar = carVariants[Random.Range(0, carVariants.Count)];
            nextCar = Instantiate(nextCar, this.transform);
            nextCar.transform.position = point.tile.transform.position;

            var yAngle = point.orientation switch
            {
                Orientation.North => 0,
                Orientation.East => 90,
                Orientation.South => 180,
                Orientation.West => -90,
                _ => throw new ArgumentOutOfRangeException(nameof(point.orientation), point.orientation, null)
            };

            nextCar.transform.Rotate(0, yAngle, 0);

            _queue.Add(nextCar);
        }
    }

    public void Increment()
    {
        _queue[^1]
            .transform.DOMove(exitTile.transform.position, 2)
            .SetSpeedBased(true)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                _queue[^1].gameObject.SetActive(false);
                _queue.RemoveAt(_queue.Count - 1);
                
                onCarLeave?.Invoke();
                
                if (_queue.Count == 0)
                {
                    foreach (var spawnPoint in path)
                    {
                        spawnPoint.tile.Occupant = null;
                    }
                    gameObject.SetActive(false);
                    return;
                }
                
                List<Tween> tweens = new List<Tween>();
                for (int i = 1; i <= _queue.Count; i++)
                {
                    var newTween = 
                        _queue[^i]
                            .transform.DOMove(path[^i].tile.transform.position, 2)
                            .SetSpeedBased(true)
                            .SetEase(Ease.InOutBack)
                            .Pause();
                    tweens.Add(newTween);
                }

                foreach (var tween in tweens)
                {
                    if (tweens.Count > tweens.IndexOf(tween) + 1)
                    {
                        tween.OnComplete(() =>
                        {
                            tweens[tweens.IndexOf(tween) + 1]?.Play();
                        });
                    }
                }

                tweens[^1].OnComplete(() =>
                {
                    if (_isInfinite)
                    {
                        SpawnNewCar();
                    }
                    _speaker.Speak();
                });
                
                tweens[0].Play();
                
            });
    }

    private void SpawnNewCar()
    {
        var nextCar = carVariants[Random.Range(0, carVariants.Count)];
        nextCar = Instantiate(nextCar, this.transform);
        nextCar.transform.position = path[0].tile.transform.position;
        nextCar.transform.Rotate(0, 90, 0);
        _queue.Insert(0, nextCar);
    }
    
    private void OnDrawGizmos()
    {
        if (path.Count == 0) return;
        foreach (SpawnPoint point in path)
        {
            Gizmos.color = point == path[^1] ? new Color(1,0,0,0.5f) : new Color(1,1,0,0.5f) ;
            Gizmos.DrawCube(point.tile.WorldPosition, Vector3.one * 0.35f);
        }
    }

    private void OnDisable()
    {
        foreach (var car in _queue)
        {
            car.GetComponent<Detector>().OccupiedTile = null;
        }
    }
    
    public void SetInfinite(bool value) => _isInfinite = value;
}

[Serializable]
public class SpawnPoint
{
    public Tile tile;
    public Orientation orientation;
}

public enum Orientation
{
    North,
    East,
    South,
    West
}
