using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour
{
    public List<Reaction> reactions;
    
    public void Listen(SoundItem item)
    {
        foreach (Reaction reaction in reactions)
        {
            if (item != reaction.key) continue;
            if (reaction.isRepeatable || !reaction.isRepeatable && !reaction.isDone)
            {
                reaction.isDone = true;
                reaction.onAcceptKey.Invoke();
            }
            break;
        }
    }
}

[Serializable]
public class Reaction
{
    public SoundItem key;
    public UnityEvent onAcceptKey;
    public bool isRepeatable = false;
    [HideInInspector] public bool isDone = false;
}
