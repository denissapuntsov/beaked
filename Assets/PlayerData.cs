using System;
using System.Collections.Generic;
using LevelStructure;
using UnityEngine;

[SelectionBase]
public class PlayerData : MonoBehaviour
{
    #region Interactions
        [Header("Interactions")] 
        public SoundItem currentSound;
        
        public Interactable FocusedInteractable => nearbyInteractables[^1];
        public List<Interactable> nearbyInteractables;

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
        #endregion

    #region Movement
        [Header("Movement")] 
        public Animator meshAnimator;
        public AnimationCurve jumpCurve;
        public float speed;
        public Vector2 walkInput;
        public Orientation orientation;
        public GridElement gridElement;
        
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
        
    #endregion
}

public enum PlayerAnimationState
{
    Idle,
    Moving,
    Speaking,
    Interacting,
    Intro
}

public enum Orientation
{
    North,
    East,
    South,
    West
}

