using System;
using DG.Tweening;
using NewTiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Cell currentCell;
    
    private Vector2 _walkInput;
    public float speed = 2f;
    private Tween _move;
    public AnimationCurve jumpCurve;

    private void Start()
    {
        TileGrid.instance.GetCellWorld(transform.position, out currentCell);
        Debug.Log(currentCell);
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        _walkInput = context.ReadValue<Vector2>();
        if (Mathf.Abs(_walkInput.x) < 0.8f && Mathf.Abs(_walkInput.y) < 0.8f) return;
        
        _walkInput = new Vector2(Mathf.Ceil(_walkInput.x), Mathf.Ceil(_walkInput.y));
        
        //if (!context.performed) return;
        
        MoveToNextTile();
    }
    
    private void MoveToNextTile()
    {
        Vector3 nextTileCoordinates = _walkInput == new Vector2(1, 1) || _walkInput == new Vector2(0, 0) ? Vector3.zero : new Vector3(_walkInput.x, 0, _walkInput.y);

        TileGrid.instance.GetCell(nextTileCoordinates + currentCell.gridPosition, out var nextCell);

        if (nextCell != null && nextCell.isAccessible)
        {
            MoveTo(nextCell);
        }
    }
    
    private void MoveTo(Cell cell)
    {
        _move?.Kill();
        //OccupiedTile.Bounce();
        cell.isAccessible = false;
        
        _move = transform
            .DOJump(cell.worldPosition, 0.3f, 1, 1 / speed)
            .SetAutoKill()
            .SetEase(jumpCurve)
            .OnComplete(() =>
            {
                currentCell.isAccessible = true;
                currentCell = cell;
                if (_walkInput == Vector2.zero)
                {
                    return;
                }

                MoveToNextTile();

            });
    }
}
