using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarQueue : MonoBehaviour
{
    public List<SpawnPoint> path;
    public List<GameObject> carVariants;
    private List<GameObject> _queue;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        foreach (SpawnPoint point in path)
        {
            var nextVariant = carVariants[Random.Range(0, carVariants.Count)];
            nextVariant = Instantiate(nextVariant, this.transform);
            nextVariant.transform.position = point.tile.transform.position;

            var yAngle = point.orientation switch
            {
                Orientation.North => 0,
                Orientation.East => 90,
                Orientation.South => 180,
                Orientation.West => -90
            };

            nextVariant.transform.Rotate(0, yAngle, 0);
        }
    }

    public void Increment()
    {
        
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
