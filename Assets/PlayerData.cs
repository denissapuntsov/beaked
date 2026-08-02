using System;
using UnityEngine;

namespace NewTiles
{
    
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance { get; private set; }


        [Header("Interactions")] 
        public SoundItem currentSound;

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
