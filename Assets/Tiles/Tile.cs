using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    public TileData data;
    public GameObject tileNormal;
    public GameObject tileBlocked;

    [SerializeField] private GameObject _occupant;

    public GameObject Occupant
    {
        get => _occupant;
        set
        {
            _occupant = value;
            IsBlocked = _occupant;
        }
    }

    private bool _isBlocked;

    public bool IsBlocked
    {
        get => _isBlocked;
        set
        {
            _isBlocked = value;
            tileNormal.gameObject.SetActive(!value);
            tileBlocked.gameObject.SetActive(value);
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
