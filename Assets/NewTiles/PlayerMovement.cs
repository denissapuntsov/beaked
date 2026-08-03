using System;
using System.Collections.Generic;
using DG.Tweening;
using NewTiles;
using PlayerState = NewTiles.PlayerState;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator meshAnimator;
    
    private Cell _currentCell;
    public Cell CurrentCell
    {
        get => _currentCell;
        set
        {
            TileGrid.instance.onPlayerCellChangeStarted?.Invoke(_currentCell);
            _currentCell = value;
            TileGrid.instance.onPlayerCellChangeEnded?.Invoke(_currentCell);
        }
    }
    
    private Vector2 _walkInput;
    public float speed = 2f;
    private Tween _move;
    public AnimationCurve jumpCurve;

    public bool isCrouching = false;

    private void Start()
    {
        TileGrid.instance.GetCellWorld(transform.position, out _currentCell);
        Debug.Log(_currentCell);
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        _walkInput = context.ReadValue<Vector2>();
        if (Mathf.Abs(_walkInput.x) < 0.8f && Mathf.Abs(_walkInput.y) < 0.8f) return;
        
        _walkInput = new Vector2(Mathf.Ceil(_walkInput.x), Mathf.Ceil(_walkInput.y));

        if (NewTiles.Player.State != NewTiles.PlayerState.Idle) return;
        if (!context.performed) return;
        
        MoveToNextTile();
    }
    
    private void MoveToNextTile()
    {
        Vector3 nextTileCoordinates = _walkInput == new Vector2(1, 1) || _walkInput == new Vector2(0, 0) ? Vector3.zero : new Vector3(_walkInput.x, 0, _walkInput.y);
        
        GetRelativeRotation(_walkInput, out int yRotation);
        meshAnimator.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        
        foreach (float f in new List<float> { 0, -0.5f, 0.5f })
        {
            TileGrid.instance.GetCell(nextTileCoordinates + CurrentCell.gridPosition + new Vector3(0, f, 0), out var nextCell);
            if (!nextCell.isAccessible || !nextCell.isSolid)
            {
                continue;
            }
            MoveTo(nextCell);
            break;
        }
    }
    
    private void MoveTo(Cell cell)
    {
        _move?.Kill();
        NewTiles.Player.State = NewTiles.PlayerState.Moving;
        _currentCell.occupyingElement.Bounce();
        cell.isAccessible = false;
        PopupManager.instance.HidePopup(gameObject);
        
        _move = transform
            .DOJump(cell.worldPosition, 0.3f, 1, 1 / speed)
            .SetAutoKill()
            .SetEase(jumpCurve)
            .OnComplete(() =>
            {
                NewTiles.Player.State = NewTiles.PlayerState.Idle;
                CurrentCell.isAccessible = true;
                CurrentCell = cell;
                if (_walkInput == Vector2.zero)
                {
                    return;
                }

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
