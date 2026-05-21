using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{
    private Tile _occupiedTile;
    public Tile OccupiedTile
    {
        get => _occupiedTile;
        private set
        {
            if (_occupiedTile != null)
            {
                _occupiedTile.IsOccupied = false;
            }
            _occupiedTile = value;
            _occupiedTile.IsOccupied = true;
        }
    }
    public float speed = 2f;
    private Tween _move;
    
    public UnityEvent<Tile> onTileChange;
    
    public PlayerState currentState = PlayerState.Idle;

    public SoundItem currentSound;
    
    public List<Speaker> speakersInRange;
    
    public void Move(Tile tile)
    {
        if (currentState != PlayerState.Idle) return;
        currentState = PlayerState.Moving;
    }
    
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
        this.StartPath(path, reachableGoal);
    }
    
    public void StartPath(List<Tile> path, Tile goal)
    {
        if (path == null) return;
        currentState = PlayerState.Moving;
        _move?.Kill();
        path[1].IsOccupied = true;
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
        Debug.Log($"Playing {currentSound.name}");
    }

    public void OnListen(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (currentState != PlayerState.Idle) return;
        if (speakersInRange.Count == 0) return;
        speakersInRange[0].Listen();
    }
}

public enum PlayerState
{
    Idle,
    Moving,
    Listening,
    Speaking
}
