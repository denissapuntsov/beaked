using System;
using UnityEngine;

namespace NewTiles
{
    [RequireComponent(typeof(GridElement))]
    public class Hideaway : MonoBehaviour
    {
        private GridElement _gridElement;
        private Cell _parentCell;

        private void Start()
        {
            _gridElement = GetComponent<GridElement>();
            TileGrid.instance.GetCellWorld(_gridElement.gridPosition, out _parentCell);
            if (_parentCell == null)
            {
                Debug.LogError($"Hideaway {gameObject.name} has not been initialized. Ensure a Cell is present at {_gridElement.gridPosition - new Vector3(0, 0.5f, 0)}.");
                return;
            }
            
            _parentCell.onPlayerEnter.AddListener(OnPlayerEnter);
            _parentCell.onPlayerExit.AddListener(OnPlayerExit);
        }

        private void OnPlayerEnter()
        {
            Debug.Log("hiding");
        }

        private void OnPlayerExit()
        {
            Debug.Log("revealed");
        }

        private void OnDisable()
        {
            _parentCell.onPlayerEnter.RemoveListener(OnPlayerEnter);
            _parentCell.onPlayerExit.RemoveListener(OnPlayerExit);
        }
    }
}
