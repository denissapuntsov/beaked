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
    public DangerPopup dangerPopup;
    
    private const int NO_ROTATION_NUMBER = 999;
    
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
            
            UpdateNeighbors();
            onTileChange?.Invoke(OccupiedTile);
        }
    }
    
    private List<Tile> _neighbors = new List<Tile>();
    public List<Tile> Neighbors => _neighbors;
    
    #region Movement
    [Header("Movement")]
    public float speed = 2f;
    private Tween _move;
    public AnimationCurve jumpCurve;
    
    public UnityEvent<Tile> onTileChange;

    private Vector2 _walkInput;
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
        
        GetOccupiedTile();
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _popup = GetComponentInChildren<ActionPopup>();
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

    public void OnWalk(InputAction.CallbackContext context)
    {
        _walkInput = context.ReadValue<Vector2>();
        if (Mathf.Abs(_walkInput.x) < 0.8f && Mathf.Abs(_walkInput.y) < 0.8f) return;
        
        _walkInput = new Vector2(Mathf.Ceil(_walkInput.x), Mathf.Ceil(_walkInput.y));
        
        if (CurrentState != PlayerState.Idle) return;
        if (!context.performed) return;
        
        MoveToNextTile();
    }

    private void MoveToNextTile()
    {
        Vector2 nextTileCoordinates = _walkInput == new Vector2(1, 1) || _walkInput == new Vector2(0, 0) ? Vector2.zero : _walkInput;
        var absoluteCoordinates = new Vector3(OccupiedTile.GridPosition.x + nextTileCoordinates.x, OccupiedTile.GridPosition.y, OccupiedTile.GridPosition.z + nextTileCoordinates.y);
        
        GetRelativeRotation(_walkInput, out int yRotation);
        meshAnimator.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        
        Tile tile = null;
        foreach (float f in new List<float> { 0f, -0.5f, 0.5f })
        {
            tile = AStar.instance.GetTileFromCoordinates(absoluteCoordinates + new Vector3(0, f, 0));
            if (tile && !tile.IsBlocked)
            {
                break;
            }
        }

        if (tile)
        {
            MoveTo(tile);
        }
    }

    private void MoveTo(Tile tile)
    {
        CurrentState = PlayerState.Moving;
        _move?.Kill();
        OccupiedTile.Bounce();
        tile.IsBlocked = true;

        dangerPopup.ClearIndicators();
        
        _move = transform
            .DOJump(tile.WorldPosition, 0.3f, 1, 1 / speed)
            .SetAutoKill()
            .SetEase(jumpCurve)
            .OnComplete(() =>
            {
                GetOccupiedTile();
                CurrentState = PlayerState.Idle;

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
    
    private void UpdateNeighbors()
    {
        _neighbors = new List<Tile>();
        foreach (Vector3 offset in new[]
                 {
                     new Vector3(1, 0, 0), 
                     new Vector3(0, 0, 1), 
                     new Vector3(0, 0, -1),
                     new Vector3(-1, 0, 0)
                 })
        {
            Tile newNeighbor = null;
            
            foreach (float f in new[] { 0f, -0.5f, 0.5f })
            {
                newNeighbor = AStar.instance.GetTileFromCoordinates(OccupiedTile.GridPosition + offset + new Vector3(0, f, 0));
                if (newNeighbor && !newNeighbor.IsBlocked)
                {
                    _neighbors.Add(newNeighbor);
                }
            }
        }
    }
    
    public void OnSpeak(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (CurrentState != PlayerState.Idle) return;
        
        if (!currentSound) return;
        
        CurrentState = PlayerState.Speaking;
        _popup.Display(currentSound);
        currentSound.audioEvent.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, InteractOnFinishedSpeaking);
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
    
    private void InteractOnFinishedSpeaking(object in_cookie, AkCallbackType in_type, object in_info)
    {
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
