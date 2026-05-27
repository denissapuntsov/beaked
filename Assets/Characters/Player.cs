using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{
    public LayerMask ignoreLayerMask;
    
    private Tile _occupiedTile;
    public Tile OccupiedTile
    {
        get => _occupiedTile;
        private set
        {
            if (_occupiedTile != null)
            {
                _occupiedTile.Occupant = null;
            }
            _occupiedTile = value;
            _occupiedTile.Occupant = gameObject;
        }
    }
    public float speed = 2f;
    private Tween _move;
    
    public UnityEvent<Tile> onTileChange;
    
    public PlayerState currentState = PlayerState.Idle;

    public SoundItem currentSound;
    
    public List<Detector> interactablesInRange;
    
    private void Update()
    {
        GetOccupiedTile();
    }

    private void GetOccupiedTile()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.parent.GetComponent<Tile>()) return;
        var hitTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
        if (hitTile == OccupiedTile) return;
        OccupiedTile = hitTile;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (currentState is not (PlayerState.Idle or PlayerState.Moving)) return;
        if (!context.started) return;

        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (!Physics.Raycast(rayOrigin, out hitInfo)) return;
        if (!hitInfo.collider.transform.parent.GetComponent<Tile>()) return;
        var hitTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
        if (hitTile == this.OccupiedTile) return;
    
        var path = AStar.instance.CalculatePath(this.OccupiedTile, hitTile, out Tile reachableGoal);
        StartPath(path, reachableGoal);
    }
    
    public void StartPath(List<Tile> path, Tile goal)
    {
        if (path == null) return;
        currentState = PlayerState.Moving;
        _move?.Kill();
        path[1].IsBlocked = true;
        _move = transform
            .DOMove(path[1].WorldPosition, speed)
            .SetSpeedBased(true)
            .SetAutoKill()
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onTileChange?.Invoke(OccupiedTile);
                if (OccupiedTile == goal)
                {
                    currentState = PlayerState.Idle;
                    return;
                }
                var nextPath = AStar.instance.CalculatePath(OccupiedTile, goal, out Tile reachableGoal);
                StartPath(nextPath, reachableGoal);
            });
    }

    public void OnSpeak(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (currentState != PlayerState.Idle) return;
        if (!currentSound) return;
        
        var neighborsInRange = AStar.instance.GetNeighborsInRange(OccupiedTile, currentSound.range);
        foreach (var neighbor in neighborsInRange)
        {
            if (!neighbor.Occupant) continue;
            Debug.Log(neighbor.Occupant);
            neighbor.Occupant.GetComponent<Listener>()?.Listen(currentSound);
        }
    }

    public void OnListen(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (currentState != PlayerState.Idle) return;
        if (interactablesInRange.Count == 0) return;
        interactablesInRange[0].GetComponent<Speaker>().Listen();
    }
}

public enum PlayerState
{
    Idle,
    Moving,
    Listening,
    Speaking
}
