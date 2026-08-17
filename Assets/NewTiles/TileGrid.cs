using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

namespace NewTiles
{
    //[ExecuteInEditMode]
    public class TileGrid : MonoBehaviour
    {
        public static TileGrid instance; 
        
        private Dictionary<Vector3, Cell> Cells = new Dictionary<Vector3, Cell>();
        private GridElement[] _gridElements;

        #region Events
        [HideInInspector]
        public UnityEvent<Cell> onPlayerCellChangeStarted, onPlayerCellChangeEnded;
        [HideInInspector]
        public UnityEvent<SoundItem> onPlayerSoundStarted, onPlayerSoundEnded;
        #endregion
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            AddListeners();
            
            Scan();
        }

        private void AddListeners()
        {
            onPlayerCellChangeStarted.AddListener(OnPlayerCellChangeStarted);
            onPlayerCellChangeEnded.AddListener(OnPlayerCellChangeEnded);
        }
        
        public void Scan()
        {
            Cells = new Dictionary<Vector3, Cell>();
            
            _gridElements = GetComponentsInChildren<GridElement>();
            
            foreach (var gridElement in _gridElements)
            {
                SetupCells(gridElement);
            }
        }
        private void SetupCells(GridElement gridElement)
        {
            gridElement.occupiedCells.Clear();
            for (int x = 0; x < gridElement.gridSize.x; x++)
            {
                for (float y = 0; y < gridElement.gridSize.y; y += 0.5f)
                {
                    for (int z = 0; z < gridElement.gridSize.z; z++)
                    {
                        var newCell = new Cell()
                        {
                            gridPosition = gridElement.transform.position + new Vector3(x, y + 1, z),
                            worldPosition = gridElement.transform.position + new Vector3(x, y + 0.5f, z),
                            occupyingElement = gridElement,
                            isAccessible = true,
                            isSolid = gridElement.isSolid
                        };
                        
                        gridElement.occupiedCells.Add(newCell);
                
                        Cells[newCell.gridPosition] = newCell;

                        if (!newCell.isSolid) continue;
                        if (Cells.ContainsKey(newCell.gridPosition - new Vector3(0, 0.5f, 0)))
                        {
                            Cells[newCell.gridPosition - new Vector3(0, 0.5f, 0)].isAccessible = false;
                        }
                    }
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            foreach (var cell in Cells.Values)
            {
                Gizmos.color = cell.isAccessible ? Color.green : Color.yellow;
                Gizmos.color = cell.isSolid ? Color.green : new Color(1f, 1, 1f, 0.1f);
                Gizmos.DrawCube(cell.gridPosition - new Vector3(0, 0.75f, 0), new Vector3(0.8f, 0.2f, 0.8f));
            }
        }

        private void OnPlayerCellChangeStarted(Cell cell)
        {
            cell.onPlayerExit.Invoke();
        }

        private void OnPlayerCellChangeEnded(Cell cell)
        {
            cell.onPlayerEnter.Invoke();
        }
        
        public void GetCell(Vector3 gridPosition, out Cell cell) => Cells.TryGetValue(gridPosition, out cell);

        public void GetCellWorld(Vector3 worldPosition, out Cell cell) => GetCell(worldPosition + new Vector3(0, 0.5f, 0), out cell);
    }
    
    [System.Serializable]
    public class Cell
    {
        public Vector3 gridPosition;
        public Vector3 worldPosition;
        [HideInInspector] public GridElement occupyingElement;
        public bool isAccessible;
        public bool isSolid = true;

        [HideInInspector] public UnityEvent onPlayerEnter = new UnityEvent();
        [HideInInspector] public UnityEvent onPlayerExit = new UnityEvent();
    }
}
