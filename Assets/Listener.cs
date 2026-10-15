using System;
using System.Collections.Generic;
using System.Linq;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

public class Listener : ChainableAction
{
    public bool isTransmitter;
    private GridElement _element;

    public UnityEvent<GridElement> onListeningStarted, onListeningComplete;
    
    [HideInInspector] public UnityEvent<ChainableAction, SoundItem> onAnySound;
    
    [SerializeField] private List<Reaction> reactions = new();
    
    private void Start()
    {
        _element = GetComponent<GridElement>();
        
        OnComplete += action => { onListeningComplete?.Invoke(_element); };
    }
    
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

    public void Trigger(ChainableAction source, SoundItem sound)
    {
        source.AddAction(this);
        Trigger(sound);
    }
    
    public void Trigger(SoundItem sound)
    {
        if (!sound) return;

        if (isTransmitter)
        {
            Debug.Log(sound.name);
            onAnySound?.Invoke(this, sound);
        }
        else
        {
            foreach (var reaction in reactions)
            {
                if (reaction.SoundItem != sound) continue;
                Debug.Log(reaction.SoundItem.name);
                reaction.Response.Invoke(this);
                break;
            }

            onListeningStarted?.Invoke(_element);
        }
        
        Complete();
    }
}

[Serializable]
public class Reaction
{
    public SoundItem SoundItem;
    public UnityEvent<ChainableAction> Response;
}
