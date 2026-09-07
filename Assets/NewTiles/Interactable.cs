using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Interactable : ChainableAction
    {
        public UnityEvent<ChainableAction> onInteract;
        
        public void Interact()
        {
            if (onInteract.GetPersistentEventCount() < 1)
            {
                Complete();
            }
            onInteract?.Invoke(this);
        }
    }

