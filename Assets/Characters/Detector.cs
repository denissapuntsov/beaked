using System;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public NPCState currentState;
    
    public int detectionRadius = 1;
    
    private List<Tile> detectionTiles = new List<Tile>();
    
    private Tile _occupiedTile;
    public Tile OccupiedTile
    {
        get => _occupiedTile;
        set
        {
            if (value == _occupiedTile) return;

            if (_occupiedTile)
            {
                _occupiedTile.Occupant = null;
            }
            
            _occupiedTile = value;
            foreach (Tile tile in detectionTiles)
            {
                tile.IsBlocked = false;
            }
            
            if (!value)
            {
                return;
            }
            
            _occupiedTile.Occupant = gameObject;
            detectionTiles = AStar.instance.GetNeighborsInRange(OccupiedTile, detectionRadius);
            foreach (Tile tile in detectionTiles)
            {
                tile.IsBlocked = true;
            }
        }
    }

    private void Update()
    {
        GetOccupiedTiles();
    }

    private void GetOccupiedTiles()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.parent.GetComponent<Tile>()) return;
        var hitTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
        if (hitTile.Occupant == gameObject) return;
        hitTile.Occupant = gameObject;
        OccupiedTile = hitTile;
    }
}

public enum NPCState
{
    Idle,
    Alerted,
    Moving,
    Speaking,
    Listening
}
