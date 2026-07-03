using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStar : MonoBehaviour
{
    
    public static AStar instance;
    public bool isDebugMode = false;
    
    public Dictionary<Vector3, Tile> tiles = new Dictionary<Vector3, Tile>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Scan();
    }

    public Tile GetTileFromCoordinates(Vector3 position)
    {
        if (tiles.ContainsKey(position)) return tiles[position];
        return null;
    }

    private void Scan()
    {
        foreach (var tile in GetComponentsInChildren<Tile>())
        {
            Debug.Log(GetComponentsInChildren<Tile>().Length);
            tiles.Add(tile.GridPosition, tile);
            tile.name += " (" + tile.GridPosition.x + ", " + tile.GridPosition.y + " , " + tile.GridPosition.z + ")";
        }

        foreach (var tile in tiles.Values)
        {
            SetNeighbors(tile);
        }
    }

    private void SetNeighbors(Tile tile)
    {
        tile.Neighbors = new Dictionary<Vector3, Tile>();
        List<Tile> neighbors = new List<Tile>();
        List<Vector3> offsets = new List<Vector3>
        {
            new (1, 0, 0),
            new (-1, 0, 0),
            new (0, 0, 1),
            new (0, 0, -1)
        };
        foreach (Vector3 offset in offsets)
        {
            var keyToTest = new Vector3( 
                tile.GridPosition.x + offset.x,
                tile.GridPosition.y + offset.y,
                tile.GridPosition.z + offset.z);
            if (tiles.TryGetValue(keyToTest, out Tile nextNeighbor))
            {
                neighbors.Add(nextNeighbor);
            }
        }

        foreach (var neighbor in neighbors)
        {
            tile.Neighbors[neighbor.GridPosition] = neighbor;
        }
    }

    public List<Tile> GetNeighborsInRange(Tile start, int radius) => GetNeighborsInRange(start, radius, TileType.None);
    
    public List<Tile> GetNeighborsInRange(Tile start, int radius, TileType filter)
    {
        List<Tile> neighbors = new List<Tile>();
        
        foreach (var tile in tiles.Values)
        {
            if ((tile.Type & filter) == tile.Type) continue;
            if (Manhattan(tile, start) <= radius)
            {
                neighbors.Add(tile);
            }
        }
        
        return neighbors;
    }

    public List<Tile> CalculatePath(Tile start, Tile goal, out Tile reachableGoal) =>
        CalculatePath(start, goal, out reachableGoal, null);
    
    public List<Tile> CalculatePath(Tile start, Tile goal, out Tile reachableGoal, List<TileType> allowedTypes)
    {
        Tile tileClosestToGoal = start;
        start = tiles[start.GridPosition];
        goal = tiles[goal.GridPosition];

        var openList = new List<Tile> { start };
        var closedList = new List<Tile>();

        Dictionary<Tile, float> gScoreMap = new Dictionary<Tile, float> { [start] = 0 };
        Dictionary<Tile, float> hScoreMap = new Dictionary<Tile, float> { [start] = Manhattan(start, goal) };

        var parentMap = new Dictionary<Tile, Tile>();

        var current = openList[0];

        while (openList.Count > 0)
        {
            current = openList[0];
            foreach (var tile in openList)
            {
                if (!gScoreMap.ContainsKey(tile) || !hScoreMap.ContainsKey(tile)) continue;
                if (hScoreMap[tile] + gScoreMap[tile] < hScoreMap[start] + gScoreMap[start])
                {
                    current = tile;
                }
                if (Manhattan(tile, goal) <= Manhattan(tileClosestToGoal, goal))
                {
                    tileClosestToGoal = tile;
                }
            }
            
            if (current == goal)
            {
                reachableGoal = goal;
                return ReconstructMap(parentMap, current);
            }
            
            openList.Remove(current);
            closedList.Add(current);
            
            foreach (var neighbor in current.Neighbors.Values)
            {
                if (allowedTypes != null && !allowedTypes.Contains(neighbor.Type)) continue;
                if (neighbor.Occupant || neighbor.IsBlocked) continue;
                if (closedList.Contains(neighbor)) continue;
                float tentativeGScore = gScoreMap[current] + Manhattan(current, neighbor);

                if (!gScoreMap.ContainsKey(neighbor) || tentativeGScore < gScoreMap[neighbor])
                {
                    // cost of already travelled tiles
                    gScoreMap[neighbor] = tentativeGScore;
                    // cost of travelling from the neighbor to the goal
                    hScoreMap[neighbor] = Manhattan(neighbor, goal);

                    parentMap[neighbor] = current;

                    if (openList.Contains(neighbor)) continue;
                    openList.Add(neighbor);
                }
            }
        }

        if (start == tileClosestToGoal)
        {
            reachableGoal = null;
            return null;
        }
        var bestPath = CalculatePath(start, tileClosestToGoal, out reachableGoal);
        return bestPath;
    }

    private List<Tile> ReconstructMap(Dictionary<Tile, Tile> parentMap, Tile current)
    {
        var path = new List<Tile> { current };

        while (parentMap.ContainsKey(current))
        {
            current = parentMap[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }

    public static float Manhattan(Tile start, Tile goal) => Mathf.Abs(start.GridPosition.x - goal.GridPosition.x) + Mathf.Abs(start.GridPosition.z -  goal.GridPosition.z);
}
