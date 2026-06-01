using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Tile _occupiedTile;
    
    private void Start()
    {
        BlockTile();
    }

    private void BlockTile()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.parent.GetComponent<Tile>()) return;
        _occupiedTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
        _occupiedTile.Occupant = gameObject;
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
