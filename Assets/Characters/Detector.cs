using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Detector : MonoBehaviour
{
    public NPCState currentState = NPCState.Idle;
    public TileType tilesToIgnore;
    private List<Tile> _detectionTiles = new List<Tile>();
    public int detectionRadius = 1;
    public string detectionLine;
    private bool _isPlayerDetected;

	[SerializeField] private bool blocksTiles = true;
    
    public UnityEvent onPlayerDetected, onPlayerLost;

    private Player _player;
    
    private Tile _occupiedTile;
    public Tile OccupiedTile
    {
        get => _occupiedTile;
        set
        {
            if (value == _occupiedTile) return;

            if (_occupiedTile && blocksTiles)
            {
                _occupiedTile.Occupant = null;
            }
            
            _occupiedTile = value;
            
            if (!value)
            {
                return;
            }
            if (blocksTiles) 
			{
            	_occupiedTile.Occupant = gameObject;
			} 
            _detectionTiles = AStar.instance.GetNeighborsInRange(OccupiedTile, detectionRadius, tilesToIgnore);
        }
    }

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _player.onTileChange.AddListener(CheckPlayerTile);
        
        onPlayerDetected.AddListener(() =>
        {
            if (detectionLine == String.Empty) return;
            Debug.Log(detectionLine);
        });
    }

    private void Update()
    {
        GetOccupiedTiles();
    }
    
    private void CheckPlayerTile(Tile playerTile)
    {
        if (_detectionTiles.Contains(playerTile))
        {
            _isPlayerDetected = true;
            _player.detectorsInRange.Add(this);
            onPlayerDetected.Invoke();
        }
        else
        {
            if (!_isPlayerDetected) return;
            _isPlayerDetected = false;
            _player.detectorsInRange.Remove(this);
            onPlayerLost.Invoke();
        }
    }

    private void GetOccupiedTiles()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.GetComponentInParent<Tile>()) return;
        var hitTile = hitInfo.collider.transform.GetComponentInParent<Tile>();
        if (hitTile.Occupant == gameObject) return;
        hitTile.Occupant = gameObject;
        OccupiedTile = hitTile;
    }

    private void OnDisable()
    {
        _player.onTileChange.RemoveListener(CheckPlayerTile);
    }
}

public enum NPCState
{
    Idle,
    Alerted,
    Moving,
    Speaking,
    Listening,
    Changing
}
