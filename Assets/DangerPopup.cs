using System;
using System.Collections.Generic;
using UnityEngine;

public class DangerPopup : MonoBehaviour
{
    [SerializeField] private GameObject dangerIndicatorPrefab;
    private List<GameObject> _spawnedIndicators;

    private void Awake()
    {
        _spawnedIndicators = new List<GameObject>();
    }

    public void AddIndicatorAt(Tile tile)
    {
        _spawnedIndicators.Add(Instantiate(dangerIndicatorPrefab, tile.WorldPosition, Quaternion.identity));
    }

    public void ClearIndicators()
    {
        foreach (var indicator in _spawnedIndicators)
        {
            Destroy(indicator);
        }
        _spawnedIndicators.Clear();
    }
}
