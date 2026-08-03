using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewTiles
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance { get; private set; }
        
        [Header("Interactions")] 
        public SoundItem currentSound;
        public Speaker FocusedSpeaker => nearbySpeakers[^1];
        public List<Speaker> nearbySpeakers;
        
        public Interactable FocusedInteractable => nearbyInteractables[^1];
        public List<Interactable> nearbyInteractables;

        [Header("Movement")] 
        public Animator meshAnimator;
        public AnimationCurve jumpCurve;
        public float speed;
        public Vector2 walkInput;
        
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
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            TileGrid.instance.GetCellWorld(transform.position, out _currentCell);
        }
    }
}
