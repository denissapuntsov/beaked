using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainableAction : MonoBehaviour
{
    public Action<ChainableAction> OnComplete { get; set; }
    public Dictionary<ChainableAction, bool> ActionsCompleteMap { get; set; } = new();

    public void AddAction(ChainableAction action)
    {
        ActionsCompleteMap.Add(action, false);
        action.OnComplete += MarkActionAsComplete;
    }

    public void MarkActionAsComplete(ChainableAction action)
    {
        ActionsCompleteMap[action] = true;
        Complete();
    }

    public void Complete()
    {
        if (ActionsCompleteMap.Values.Contains(false)) return;
        
        foreach (var reactionType in ActionsCompleteMap.Keys)
        {
            reactionType.OnComplete -= MarkActionAsComplete;
        }
        ActionsCompleteMap.Clear();
        OnComplete?.Invoke(this);
    }
}
