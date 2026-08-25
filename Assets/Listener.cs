using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Listener : ChainableAction
{
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
}

[Serializable]
public class Reaction
{
    public SoundItem SoundItem;
    public UnityEvent<Listener> Response;
}
