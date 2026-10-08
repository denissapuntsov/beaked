using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Interactable : ChainableAction
    {
        private GridElementControls _controls;
        [SerializeField] private float preReactionTime = 0f;
        public PeckMaterial interactableMaterial;
        public UnityEvent onPreInteract;
        public UnityEvent<ChainableAction> onInteract;

        private void Awake()
        {
            _controls = GetComponent<GridElementControls>();
        }

        public void Interact()
        {
            SoundManager.instance.SetPeckMaterial(interactableMaterial);
            _controls?.ReactToPeck();
            onPreInteract?.Invoke();
            Invoke(nameof(OnInitialReactionOver), preReactionTime);
        }
        
        public void OnInitialReactionOver()
        {
            if (onInteract.GetPersistentEventCount() < 1)
            {
                Complete();
            }
            onInteract?.Invoke(this);
        }
    }

    public enum PeckMaterial
    {
        Base,
        Rock,
        Metal
    }