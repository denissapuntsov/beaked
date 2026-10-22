using System;
using System.Collections.Generic;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class PlayerData : MonoBehaviour
{
    #region Inventory

    public List<SoundItem> soundHistory = new();
    
    #endregion
    
    #region Interactions

    [Header("Interactions")] 
    private SoundItem _currentSound;

    public SoundItem CurrentSound
    {
        get => _currentSound;
        set
        {
            _currentSound = value;
            FindAnyObjectByType<MenuCanvas>().SetVisibility(true, _currentSound != null);
        }
    }
        
        //public Interactable FocusedInteractable => nearbyInteractables[^1];
        //public List<Interactable> nearbyInteractables;

        [field: SerializeField]
        private Cell currentCell;
        public Cell CurrentCell
        {
            get => currentCell;
            set
            {
                currentCell.IsAccessible = true;
                currentCell = value;
                /*Debug.Log(value);*/
                currentCell.IsAccessible = false;
            }
        }

        public UnityEvent onPeckBegin;
        #endregion

    #region Movement
        [Header("Movement")] 
        public Animator meshAnimator;
        public AnimationCurve jumpCurve;
        public float speed;
        public Vector2 walkInput;
        public Orientation orientation;
        public GridElement gridElement;
        public UnityEvent onStepBegin;
        
        #endregion
        
    #region Animation
    [Header("Animation")]

    public PlayerAnimationState AnimationState
    {
        set
        {
            if (!meshAnimator.HasState(0, Animator.StringToHash(value.ToString()))) return;
            meshAnimator.Play(Animator.StringToHash(value.ToString()), 0, 0);
        }
    }

    private void Awake()
    {
        CurrentSound = null;
    }

    #endregion
}

public enum PlayerAnimationState
{
    //Idle,
    Moving,
    Speaking,
    Interacting,
    Learning,
    Intro
}

public enum Orientation
{
    North,
    East,
    South,
    West
}

