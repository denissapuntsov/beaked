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
        private set
        {
            if (value == _occupiedTile) return;
            if (_occupiedTile != null)
            {
                _occupiedTile.IsOccupied = false;
            }
            
            _occupiedTile = value;
            _occupiedTile.IsOccupied = true;

            foreach (Tile tile in detectionTiles)
            {
                tile.IsOccupied = false;
            }
            
            //detectionTiles.Clear();
            detectionTiles = AStar.instance.GetNeighborsInRange(OccupiedTile, detectionRadius);
            
            foreach (Tile tile in detectionTiles)
            {
                tile.IsOccupied = true;
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
        if (hitTile == OccupiedTile) return;
        OccupiedTile = hitTile;
    }
}

public enum NPCState
{
    Idle,
    Moving,
    Speaking,
    Listening
}
