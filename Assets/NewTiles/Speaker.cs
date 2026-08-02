using System;
using UnityEngine;

namespace NewTiles
{
    [RequireComponent(typeof(TileAttachment))]
    public class Speaker : MonoBehaviour
    {
		[SerializeField] private SoundItem soundItem;     
        public SoundItem SoundItem => soundItem;

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
                PlayerData.Instance.nearbySpeakers.Remove(this);
                return;
            }
            PlayerData.Instance.nearbySpeakers.Add(this);
            PopupManager.instance.ShowPopup(Player.instance.gameObject, "Q");
        }

        private void OnDisable()
        {
            TileGrid.instance.onPlayerCellChangeEnded.RemoveListener(CheckForPlayerCell);
        }
    }
}
