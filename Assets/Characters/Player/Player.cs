using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[SelectionBase] [RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private AudioSource _audioSource;
    
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
            IsHidden = (_occupiedTile.Type & TileType.Hideout) != 0;
            _occupiedTile.Occupant = gameObject;
        }
    }
    
    #region Movement
    [Header("Movement")]
    public float speed = 2f;
    private Tween _move;
    public AnimationCurve jumpCurve;
    
    public UnityEvent<Tile> onTileChange;
    #endregion
    
    #region Interaction
    [Header("Interaction")]
    public SoundItem currentSound;
    
    public List<Detector> detectorsInRange;

    private ActionPopup _popup;
    #endregion
    
    #region State

    [Header("State")] 
    
    [SerializeField] private Animator meshAnimator;
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Speaking = Animator.StringToHash("Speaking");
    
    private PlayerState _currentState = PlayerState.Idle;

    public PlayerState CurrentState
    {
        get => _currentState;
        set
        {
            if (value == _currentState) return;
            _currentState = value;

            if (!meshAnimator.HasState(0, Animator.StringToHash(value.ToString()))) return;
            meshAnimator.Play(Animator.StringToHash(value.ToString()), 0, 0);
        }
    }
    
    private bool _isHidden;
    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            if (_isHidden == value) return;
            _isHidden = value;
            ToggleCrouch();
        }
    }
    #endregion

    public static Player instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _popup = GetComponentInChildren<ActionPopup>();
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

    /*public void OnMove(InputAction.CallbackContext context)
    {
        if (CurrentState is not (PlayerState.Idle /*or PlayerState.Moving#1#)) return;
        if (!context.started) return;

        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (!Physics.Raycast(rayOrigin, out hitInfo)) return;
        if (!hitInfo.collider.transform.GetComponentInParent<Tile>()) return;
        var hitTile = hitInfo.collider.transform.GetComponentInParent<Tile>();
        if (hitTile == this.OccupiedTile) return;
    
        var path = AStar.instance.CalculatePath(this.OccupiedTile, hitTile, out Tile reachableGoal);
        StartPath(path, reachableGoal);
    }*/

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (CurrentState != PlayerState.Idle) return;
        if (!context.performed) return;
        
        var input = context.ReadValue<Vector2>();
        input = new Vector2(Mathf.Ceil(input.x), Mathf.Ceil(input.y));
        

        Vector2 nextTileCoordinates = input == new Vector2(1, 1) || input == new Vector2(0, 0) ? Vector2.zero : input;
        var absoluteCoordinates = new Vector3(OccupiedTile.GridPosition.x + nextTileCoordinates.x, 0, OccupiedTile.GridPosition.z + nextTileCoordinates.y);
        var tile = AStar.instance.GetTileFromCoordinates(absoluteCoordinates);

        if (!tile) return;
        MoveToTile(tile);
    }

    private void MoveToTile(Tile tile)
    {
        CurrentState = PlayerState.Moving;
        _move?.Kill();
        OccupiedTile.Bounce();
        tile.IsBlocked = true;
        
        GetRelativeRotationToNextTile(tile, out int yRotation);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        
        _move = transform
            .DOJump(tile.WorldPosition, 0.2f, 1, 1 / speed)
            .SetAutoKill()
            .SetEase(jumpCurve)
            .OnComplete(() =>
            {
                CurrentState = PlayerState.Idle;
            });
    }
    
    /*public void StartPath(List<Tile> path, Tile goal)
    {
        if (path == null) return;
        CurrentState = PlayerState.Moving;
        _move?.Kill();
        OccupiedTile.Bounce();
        path[1].IsBlocked = true;
        
        GetRotationRelativeToNextTile(path, out int yRotation);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        
        _move = transform
            .DOJump(path[1].WorldPosition, 0.2f, 1, 1 / speed)
            .SetAutoKill()
            .SetEase(jumpCurve)
            .OnComplete(() =>
            {
                CurrentState = PlayerState.Idle;
                onTileChange?.Invoke(OccupiedTile);
                if (OccupiedTile == goal)
                {
                    return;
                }
                var nextPath = AStar.instance.CalculatePath(OccupiedTile, goal, out Tile reachableGoal);
                StartPath(nextPath, reachableGoal);
            });
    }*/

    private void GetRelativeRotationToNextTile(Tile nextTile, out int yRotation)
    {
        switch (new Vector2(OccupiedTile.GridPosition.x - nextTile.GridPosition.x,
                    OccupiedTile.GridPosition.z - nextTile.GridPosition.z))
        {
            case var v when v == new Vector2(0, 1):
                yRotation = 90;
                break;
            case var v when v == new Vector2(0, -1):
                yRotation = -90;
                break;
            case var v when v == new Vector2(1, 0):
                yRotation = 180;
                break;
            default:
                yRotation = 0;
                break;
        }
    }

    /*private static void GetRotationRelativeToNextTile(List<Tile> path, out int yRotation)
    {
        switch (new Vector2(path[0].GridPosition.x - path[1].GridPosition.x,
                    path[0].GridPosition.z - path[1].GridPosition.z))
        {
            case var v when v == new Vector2(0, 1):
                yRotation = 90;
                break;
            case var v when v == new Vector2(0, -1):
                yRotation = -90;
                break;
            case var v when v == new Vector2(1, 0):
                yRotation = 180;
                break;
            default:
                yRotation = 0;
                break;
        }
    }*/

    public void OnSpeak(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (CurrentState != PlayerState.Idle) return;
        if (!currentSound) return;
        
        CurrentState = PlayerState.Speaking;
        
        _popup.Display(currentSound);
        
        _audioSource.clip = currentSound.clip;
        _audioSource.Play();
        StartCoroutine(InteractOnFinishedSpeaking());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (CurrentState != PlayerState.Idle) return;

        Interactable interactableNeighbor = null;
        
        foreach (var detector in detectorsInRange)
        {
            if (!detector.GetComponent<Interactable>()) continue;
            interactableNeighbor = detector.GetComponent<Interactable>();
        }

        if (!interactableNeighbor) return;
        CurrentState = PlayerState.Interacting;
        interactableNeighbor.Use();
        CurrentState = PlayerState.Idle;
    }
    
    private IEnumerator InteractOnFinishedSpeaking()
    {
        var waitForClipRemainingTime = new WaitForSeconds(_audioSource.GetClipRemainingTime());
        yield return waitForClipRemainingTime;

        _popup.Hide();
        
        var neighborsInRange = AStar.instance.GetNeighborsInRange(OccupiedTile, currentSound.range);
        foreach (var neighbor in neighborsInRange)
        {
            if (!neighbor.Occupant) continue;
            Debug.Log(neighbor.Occupant);
            neighbor.Occupant.GetComponent<Listener>()?.Listen(currentSound);
        }
        CurrentState = PlayerState.Idle;
    }

    public void OnListen(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (CurrentState != PlayerState.Idle) return;
        if (detectorsInRange.Count == 0) return;

        foreach (var detector in detectorsInRange)
        {
            if (!detector.GetComponent<Speaker>()) continue;
            detector.GetComponent<Speaker>()?.Listen();
        }
    }

    private void ToggleCrouch()
    {
        transform.DOScaleY(IsHidden ? 0.75f : 1, 3f).SetSpeedBased(true);
    }
}

public enum PlayerState
{
    Idle,
    Moving,
    Listening,
    Speaking,
    Interacting
}
