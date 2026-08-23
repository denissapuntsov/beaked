using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour, IChainableAction
{
    public Action<IChainableAction> OnComplete { get; set; }
    public Dictionary<IChainableAction, bool> ActionsCompleteMap { get; set; } = new();
    
    [SerializeField] private List<Reaction> reactions = new();
    
    public bool CheckForSound(SoundItem sound)
    {
        foreach (var reaction in reactions)
        {
            if (reaction?.SoundItem == sound)
            {
                return true;
            }
        }

        return false;
    }
    
    public void Trigger(SoundItem sound)
    {
        foreach (var reaction in reactions)
        {
            if (reaction.SoundItem != sound) continue;
            reaction.Response.Invoke(this);
            break;
        }
    }

    public void AddReaction(IChainableAction chainableAction)
    {
        ActionsCompleteMap.Add(chainableAction, false);
        chainableAction.OnComplete += MarkReactionComplete;
    }

    private void MarkReactionComplete(IChainableAction chainableAction)
    {
        ActionsCompleteMap[chainableAction] = true;
        CheckForCompletion();
    }
    
    public void CheckForCompletion()
    {
        if (ActionsCompleteMap.Values.Contains(false)) return;
        
        foreach (var reactionType in ActionsCompleteMap.Keys)
        {
            reactionType.OnComplete -= MarkReactionComplete;
        }
        ActionsCompleteMap.Clear();
        OnComplete?.Invoke(this);
    }
}

[Serializable]
public class Reaction
{
    public SoundItem SoundItem;
    public UnityEvent<Listener> Response;
}
