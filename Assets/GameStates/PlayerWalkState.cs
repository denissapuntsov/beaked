using System.Collections.Generic;
using DG.Tweening;
using NewTiles;
using Cell = NewTiles.Cell;
// TODO: change to work with MVC TileGrid architecture
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : GameState
{
    private PlayerInputActions _actions;
    private Tween _move;
    
    public PlayerWalkState(PlayerInputActions actions) : base()
    {
        _actions = actions;
    }
    
    public override void Enter()
    {
        _actions.Player.Walk.performed += OnWalk;
        var walkInput = new Vector2(Mathf.Ceil(PlayerData.Instance.walkInput.x), Mathf.Ceil(PlayerData.Instance.walkInput.y));
        TryTileAtInput(walkInput);
    }

    public override void Exit()
    {
        base.Exit();
        _move?.Kill();
        _actions.Player.Walk.performed -= OnWalk;
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        PlayerData.Instance.walkInput = context.ReadValue<Vector2>();
    }
    
    private void TryTileAtInput(Vector2 input)
    {
        Vector3 nextTileCoordinates = input == new Vector2(1, 1) || input == new Vector2(0, 0) ? Vector3.zero : new Vector3(input.x, 0, input.y);
            
        GetRelativeRotation(input, out int yRotation);
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

        Complete();
    }
    
    private void MoveTo(NewTiles.Cell cell)
    {
        PlayerData.Instance.AnimationState = PlayerAnimationState.Moving;
        PlayerData.Instance.transform.SetParent(cell.occupyingElement.transform);
        _move?.Kill();
        PlayerData.Instance.CurrentCell.occupyingElement.Bounce();
        cell.isAccessible = false;
        PopupManager.instance.HidePopup(PlayerData.Instance.gameObject);
            
        _move = PlayerData.Instance.gameObject.transform
            .DOJump(cell.worldPosition, 0.3f, 1, 1 / PlayerData.Instance.speed)
            .SetAutoKill()
            .SetEase(PlayerData.Instance.jumpCurve)
            .OnComplete(() =>
            {
                PlayerData.Instance.CurrentCell.isAccessible = true;
                PlayerData.Instance.CurrentCell = cell;
                var walkInput = new Vector2(Mathf.Ceil(PlayerData.Instance.walkInput.x), Mathf.Ceil(PlayerData.Instance.walkInput.y));
                if (walkInput == Vector2.zero)
                {
                    Complete();
                    return;
                }
                PlayerData.Instance.transform.SetParent(null);
                TryTileAtInput(walkInput);
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
