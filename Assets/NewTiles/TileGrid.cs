using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace NewTiles
{
    [ExecuteInEditMode]
    public class TileGrid : MonoBehaviour
    {
        public static TileGrid instance; 
        
        private Dictionary<Vector3, Cell> Cells = new Dictionary<Vector3, Cell>();
        [SerializeField] private List<Cell> cells = new List<Cell>();
        
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
            
            Scan();
        }
        public void Scan()
        {
            Cells = new Dictionary<Vector3, Cell>();
            cells.Clear();
            
            foreach (var gridElement in GetComponentsInChildren<GridElement>())
            {
                SetupCells(gridElement);
            }
        }
        private void SetupCells(GridElement gridElement)
        {
            for (int x = 0; x < gridElement.gridSize.x; x++)
            {
                for (float y = 0; y < gridElement.gridSize.y; y += 0.5f)
                {
                    for (int z = 0; z < gridElement.gridSize.z; z++)
                    {
                        var newCell = new Cell()
                        {
                            gridPosition = gridElement.transform.localPosition + new Vector3(x, y + 1, z),
                            worldPosition = gridElement.transform.position + new Vector3(x, y + 0.5f, z),
                            occupyingElement = gridElement,
                            isAccessible = true
                        };
                
                        Cells[newCell.gridPosition] = newCell;
                        cells.Add(newCell);

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
                Gizmos.DrawCube(cell.gridPosition - new Vector3(0, 0.75f, 0), new Vector3(0.8f, 0.2f, 0.8f));
            }
        }
        
        public void GetCell(Vector3 gridPosition, out Cell cell) => Cells.TryGetValue(gridPosition, out cell);

        public void GetCellWorld(Vector3 worldPosition, out Cell cell) => GetCell(worldPosition + new Vector3(0, 0.5f, 0), out cell);
    }
    
    [System.Serializable]
    public class Cell
    {
        public Vector3 gridPosition;
        public Vector3 worldPosition;
        public GridElement occupyingElement;
        public bool isAccessible;
    }
}
