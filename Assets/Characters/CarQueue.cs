using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Speaker))]
public class CarQueue : MonoBehaviour
{
    public List<SpawnPoint> path;
    public List<GameObject> carVariants;
    private List<GameObject> _queue = new List<GameObject>();

    private void Start()
    {
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
