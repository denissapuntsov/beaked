using UnityEngine;
using UnityEngine.Events;

namespace NewTiles
{
    [RequireComponent(typeof(TileAttachment))]
    public class Interactable : MonoBehaviour
    {
        public UnityEvent onInteract;
        private TileAttachment _attachment;
        
        private void Start()
        {
            _attachment = GetComponent<TileAttachment>();
            TileGrid.instance.onPlayerCellChangeEnded.AddListener(CheckForPlayerCell);
        }
        
        private void CheckForPlayerCell(Cell cell)
        {
            if (Mathf.Abs(cell.gridPosition.x - _attachment.ParentCell.gridPosition.x) +
                Mathf.Abs(cell.gridPosition.z - _attachment.ParentCell.gridPosition.z) > 1 ||
                Mathf.Abs(cell.gridPosition.y - _attachment.ParentCell.gridPosition.y) > 0.5f)
            {
                PlayerData.Instance.nearbyInteractables.Remove(this);
                return;
            }
            PlayerData.Instance.nearbyInteractables.Add(this);
            PopupManager.instance.ShowPopup(Player.instance.gameObject, "F");
        }
        
        public void Interact()
        {
            PopupManager.instance.HidePopup(Player.instance.gameObject);
            onInteract.Invoke();
        }
    }
}
