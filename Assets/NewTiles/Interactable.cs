using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Interactable : ChainableAction
    {
        public InteractableStateMachine StateMachine;

        public UnityEvent<ChainableAction> onPeck;
        public UnityEvent<ChainableAction, SoundItem> onListen;
        
        private GridElementControls _controls;
        [SerializeField] private float preReactionTime = 0f;
        public PeckMaterial interactableMaterial;
        
        private void Awake()
        {
            _controls = GetComponent<GridElementControls>();
            StateMachine = new InteractableStateMachine();
        }

        public void OnPeck()
        {
            onPeck?.Invoke(this);
            if (onPeck?.GetPersistentEventCount() == 0)
            {
                Complete();
            }
        }

        public void OnListen(SoundItem sound)
        {
            onListen?.Invoke(this, sound);
        }

        /*public void Interact()
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
        }*/
    }

    public enum PeckMaterial
    {
        Base,
        Rock,
        Metal
    }