using System.Collections.Generic;
using DG.Tweening;
using LevelStructure;
// TODO: change to work with MVC TileGrid architecture
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : GameState
{
    private PlayerInputActions _actions;
    private PlayerData _data;
    private TileGridController _controller;
    private Tween _move;
    
    public PlayerWalkState(PlayerInputActions actions, PlayerData data, TileGridController controller) : base()
    {
        _actions = actions;
        _data = data;
        _controller = controller;
    }
    
    public override void Enter()
    {
        _actions.Player.Walk.performed += OnWalk;
        var walkInput = new Vector2(Mathf.Ceil(_data.walkInput.x), Mathf.Ceil(_data.walkInput.y));
        TryCellAtInput(walkInput);
    }

    public override void Exit()
    {
        base.Exit();
        _move?.Kill();
        _actions.Player.Walk.performed -= OnWalk;
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        _data.walkInput = context.ReadValue<Vector2>();
    }
    
    private void TryCellAtInput(Vector2 input)
    {
        Vector3 nextCellCoordinates = input == new Vector2(1, 1) || input == new Vector2(0, 0) ? Vector3.zero : new Vector3(input.x, 0, input.y);
            
        GetRelativeRotation(input, out int yRotation);
        
        _data.meshAnimator.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            
        foreach (float f in new List<float> { 0, 0.5f, -0.5f })
        {
            var nextCell = _controller.TryGetCellAtLocation(nextCellCoordinates + _data.CurrentCell.GridPosition + new Vector3(0, f, 0));
            //TileGrid.instance.GetCellAt(nextTileCoordinates + _data.CurrentCell.gridPosition + new Vector3(0, f, 0), out var nextCell);
            if (nextCell is not { IsAccessible: true, IsSolid: true }) continue;
            MoveTo(nextCell);
            return;
        }

        Complete();
    }
    
    private void MoveTo(Cell cell)
    {
        ChangeFootstepSound();

        _data.onStepBegin?.Invoke();
        _data.AnimationState = PlayerAnimationState.Moving;
        _move?.Kill();
        
        //_controller.RemoveElement(_data.gridElement);
        cell.IsAccessible = false;
        _controller.BounceCell(_data.CurrentCell);
        
        _move = _data.gameObject.transform
            .DOJump(cell.GridPosition, 0.3f, 1, 1 / _data.speed)
            .SetAutoKill()
            .SetEase(_data.jumpCurve)
            .OnComplete(() =>
            {
                _controller.RemoveElement(_data.gridElement);
                _controller.AddElement(_data.gridElement);
                _data.CurrentCell.IsAccessible = true;
                
                //_controller.AddElement(_data.gridElement);
                _data.CurrentCell = cell;
                var walkInput = new Vector2(Mathf.Ceil(_data.walkInput.x), Mathf.Ceil(_data.walkInput.y));
                if (walkInput == Vector2.zero)
                {
                    Complete();
                    return;
                }
                _data.transform.SetParent(null);
                TryCellAtInput(walkInput);
            });
    }

    private void ChangeFootstepSound()
    {
        var tileBelow = _controller.CellToElementMap[_data.CurrentCell];
        SoundManager.instance.SetFootstepMaterial(tileBelow.stepMaterial);
    }

    private void GetRelativeRotation(Vector2 walkInput, out int yRotation)
    {
        switch (walkInput)
        {
            case var v when v == new Vector2(0, -1):
                yRotation = 90;
                _data.orientation = Orientation.South;
                break;
            case var v when v == new Vector2(0, 1):
                yRotation = -90;
                _data.orientation = Orientation.North;
                break;
            case var v when v == new Vector2(-1, 0):
                yRotation = 180;
                _data.orientation = Orientation.West;
                break;
            default:
                yRotation = 0;
                _data.orientation = Orientation.East;
                break;
        }
    }
}
