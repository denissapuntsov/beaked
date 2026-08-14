using UnityEngine;
using UnityEngine.Events;

namespace NewTiles
{
	[RequireComponent(typeof(GridElement))]
    public class TileAttachment : MonoBehaviour
    {
        public UnityEvent onEnter, onExit;
        
        private GridElement _gridElement;
        private Cell _parentCell;

        public Cell ParentCell
        {
            get => _parentCell;
            set
            {
                _parentCell?.onPlayerEnter.RemoveListener(OnPlayerEnter);
                _parentCell?.onPlayerExit.RemoveListener(OnPlayerExit);
                _parentCell = value;
                if (value == null) return;
                transform.SetParent(_parentCell.occupyingElement.pivot);
                _parentCell.onPlayerEnter.AddListener(OnPlayerEnter);
                _parentCell.onPlayerExit.AddListener(OnPlayerExit);
            }
        }

        private void Start()
        {
            _gridElement = GetComponent<GridElement>();
            TileGrid.instance.GetCellWorld(_gridElement.GridPosition, out var newParentCell);
            ParentCell = newParentCell;
            if (ParentCell == null)
            {
                Debug.LogError($"Hideaway {gameObject.name} has not been initialized. Ensure a Cell is present at {_gridElement.GridPosition - new Vector3(0, 0.5f, 0)}.");
                return;
            }

            transform.SetParent(ParentCell.occupyingElement.pivot);
            var localPos = transform.localPosition;
            Debug.Log(gameObject.name + " " + localPos);
            localPos.y = 1f;
            transform.localPosition = localPos;
        }
        
        private void OnPlayerEnter() => onEnter?.Invoke();

        private void OnPlayerExit() => onExit?.Invoke();

        private void OnDisable()
        {
            _parentCell?.onPlayerEnter.RemoveListener(OnPlayerEnter);
            _parentCell?.onPlayerExit.RemoveListener(OnPlayerExit);
        }
    }
}
