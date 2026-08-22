using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour
{
    public Action<Listener> OnComplete { get; set; }
    
    [SerializeField] private List<Reaction> reactions = new();
    
    private Dictionary<IReactionType, bool> _reactionCompletionMap = new();

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

    public void AddReaction(IReactionType reactionType)
    {
        _reactionCompletionMap.Add(reactionType, false);
        reactionType.OnComplete += MarkReactionAsComplete;
    }

    private void MarkReactionAsComplete(IReactionType reactionType)
    {
        _reactionCompletionMap[reactionType] = true;
        CheckForCompletion();
    }
    
    private void CheckForCompletion()
    {
        if (_reactionCompletionMap.Values.Contains(false)) return;
        
        foreach (var reactionType in _reactionCompletionMap.Keys)
        {
            reactionType.OnComplete -= MarkReactionAsComplete;
        }
        _reactionCompletionMap.Clear();
        OnComplete?.Invoke(this);
    }
}

[Serializable]
public class Reaction
{
    public SoundItem SoundItem;
    public UnityEvent<Listener> Response;
}
