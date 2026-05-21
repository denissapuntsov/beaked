using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Start()
    {
        OccupyTile();
    }

    private void OccupyTile()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.parent.GetComponent<Tile>()) return;
        var hitTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
        hitTile.IsOccupied = true;
    }
}
