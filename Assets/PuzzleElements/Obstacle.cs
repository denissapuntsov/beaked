using System;
using UnityEngine;

[SelectionBase]
public class Obstacle : MonoBehaviour
{
    private Tile _occupiedTile;
    public Tile OccupiedTile => _occupiedTile;
    
    private void Start()
    {
        GetOccupiedTile();
        BlockTile();
    }

    private void GetOccupiedTile()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.GetComponentInParent<Tile>()) return;
        _occupiedTile = hitInfo.collider.transform.GetComponentInParent<Tile>();
    }

    public void BlockTile()
    {
        _occupiedTile.Occupant = gameObject;
    }

    public void UnblockTile()
    {
        _occupiedTile.Occupant = null;
        _occupiedTile = null;
    }

    private void OnDisable()
    {
        if (_occupiedTile == null) return;
        if (_occupiedTile.gameObject.activeSelf)
        {
            _occupiedTile.Occupant = null;
        }
    }
}
