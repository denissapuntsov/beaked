using System;
using UnityEngine;

/*namespace NewTiles
{
    [RequireComponent(typeof(TileAttachment))]
    public class Speaker : MonoBehaviour
    {
		[SerializeField] private SoundItem soundItem;     
        public SoundItem SoundItem => soundItem;

        public bool IsSpeaking
        {
            get; private set;
        }
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
        }

        public void Speak()
        {
            PopupManager.instance.ShowPopup(gameObject, "!");
            IsSpeaking = true;
            if (PlayerData.Instance.nearbySpeakers.Contains(this))
            {
                PopupManager.instance.ShowPopup(PlayerData.Instance.gameObject, "Q");
            }
            soundItem.audioEvent.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnFinishedSpeaking);
        }
        
        private void OnFinishedSpeaking(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
        {
            IsSpeaking = false;
            PopupManager.instance.HidePopup(gameObject);
            
            if (PlayerData.Instance.nearbySpeakers.Contains(this))
            {
                PopupManager.instance.HidePopup(Player.instance.gameObject);
            }
        }

        private void OnDisable()
        {
            TileGrid.instance.onPlayerCellChangeEnded.RemoveListener(CheckForPlayerCell);
        }
    }
}*/
