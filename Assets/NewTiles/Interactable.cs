using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Interactable : ChainableAction
    {
        public UnityEvent<ChainableAction> onInteract;
        
        public void Interact()
        {
            onInteract.Invoke(this);
        }

        /*public Action<IChainableAction> OnComplete { get; set; }
        public Dictionary<IChainableAction, bool> ActionsCompleteMap { get; set; } = new();
        public void AddAction(IChainableAction action)
        {
            ActionsCompleteMap.Add(action, false);
            action.OnComplete += MarkActionAsComplete;
        }

        public void MarkActionAsComplete(IChainableAction action)
        {
            ActionsCompleteMap[action] = true;
            Complete();
        }

        public void Complete()
        {
            if (ActionsCompleteMap.ContainsValue(false))
            {
                return;
            }
        
            foreach (var action in ActionsCompleteMap.Keys)
            {
                action.OnComplete -= MarkActionAsComplete;
            }
            ActionsCompleteMap.Clear();
        
            OnComplete?.Invoke(this);
        }*/
    }

