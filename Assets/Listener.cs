using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour
{
    public Action OnComplete { get; set; }
    
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
            reaction.Response.Invoke();
            break;
        }
    }
}

[Serializable]
public class Reaction
{
    public SoundItem SoundItem;
    public UnityEvent Response;
}
