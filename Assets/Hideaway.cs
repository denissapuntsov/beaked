using UnityEngine;

namespace NewTiles
{
    [RequireComponent(typeof(TileAttachment))]
    public class Hideaway : MonoBehaviour
    {
        private GridElement _gridElement;
        private TileAttachment _tileAttachment;
        private Cell _parentCell;

        private void Start()
        {
            _tileAttachment.ParentCell.onPlayerEnter.AddListener(OnPlayerEnter);
            _tileAttachment.ParentCell.onPlayerExit.AddListener(OnPlayerExit);
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
            _parentCell?.onPlayerEnter.RemoveListener(OnPlayerEnter);
            _parentCell?.onPlayerExit.RemoveListener(OnPlayerExit);
        }
    }
}
