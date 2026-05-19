using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    public TileData data;
    public GameObject tileNormal;
    public GameObject tileBlocked;
    private bool _isOccupied = false;

    public bool IsOccupied
    {
        get => _isOccupied;
        set
        {
            _isOccupied = value;
            tileNormal.SetActive(!value);
            tileBlocked.SetActive(value);
        }
    }

    private void Start()
    {
        tileNormal.gameObject.SetActive(true);
        tileBlocked.gameObject.SetActive(false);
    }

    public Vector3 WorldPosition => transform.position;
    public Vector3 GridPosition => transform.localPosition;
    
    public Dictionary<Vector3, Tile> Neighbors { get; set; }
}
