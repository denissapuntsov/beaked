using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewTiles.PlayerStateMachine
{
    public class MovingState : BaseState
    {
        public MovingState(PlayerStateMachine machine) : base(machine) { }
        
        private Tween _move;
        private Vector2 _walkInput;

        public override void Enter()
        {
            _walkInput = new Vector2(Mathf.Ceil(PlayerData.Instance.walkInput.x), Mathf.Ceil(PlayerData.Instance.walkInput.y));
            MoveToNextTile();
        }
        
        private void MoveToNextTile()
        {
            Vector3 nextTileCoordinates = _walkInput == new Vector2(1, 1) || _walkInput == new Vector2(0, 0) ? Vector3.zero : new Vector3(_walkInput.x, 0, _walkInput.y);
            
            GetRelativeRotation(_walkInput, out int yRotation);
            PlayerData.Instance.meshAnimator.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            
            foreach (float f in new List<float> { 0, -0.5f, 0.5f })
            {
                TileGrid.instance.GetCellAt(nextTileCoordinates + PlayerData.Instance.CurrentCell.gridPosition + new Vector3(0, f, 0), out var nextCell);
                if (nextCell is { isAccessible: true, isSolid: true })
                {
                    MoveTo(nextCell);
                    return;
                }
            }
            
            _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
        }
    
        private void MoveTo(Cell cell)
        {
            PlayerData.Instance.AnimationState = PlayerAnimationState.Moving;
            _playerStateMachine.transform.SetParent(cell.occupyingElement.transform);
            _move?.Kill();
            PlayerData.Instance.CurrentCell.occupyingElement.Bounce();
            cell.isAccessible = false;
            PopupManager.instance.HidePopup(_playerStateMachine.gameObject);
            
            _move = _playerStateMachine.gameObject.transform
                .DOJump(cell.worldPosition, 0.3f, 1, 1 / PlayerData.Instance.speed)
                .SetAutoKill()
                .SetEase(PlayerData.Instance.jumpCurve)
                .OnComplete(() =>
                {
                    PlayerData.Instance.CurrentCell.isAccessible = true;
                    PlayerData.Instance.CurrentCell = cell;
                    _walkInput = new Vector2(Mathf.Ceil(PlayerData.Instance.walkInput.x), Mathf.Ceil(PlayerData.Instance.walkInput.y));
                    if (_walkInput == Vector2.zero)
                    {
                        _playerStateMachine.CurrentState = _playerStateMachine.IdleState;
                        return;
                    }
                    _playerStateMachine.transform.SetParent(null);

                    MoveToNextTile();
                });
        }
        
        private void GetRelativeRotation(Vector2 walkInput, out int yRotation)
        {
            switch (walkInput)
            {
                case var v when v == new Vector2(0, -1):
                    yRotation = 90;
                    break;
                case var v when v == new Vector2(0, 1):
                    yRotation = -90;
                    break;
                case var v when v == new Vector2(-1, 0):
                    yRotation = 180;
                    break;
                default:
                    yRotation = 0;
                    break;
            }
        }
    }
}
