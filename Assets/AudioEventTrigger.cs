using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AudioEventTrigger : MonoBehaviour
{
    [SerializeField] public List<AudioEventData> audioEvents;
    
    public void Play(string eventName)
    {
        foreach (var audioEvent in audioEvents)
        {
            if (audioEvent.name != eventName) continue;
            audioEvent.audioEvent.Post(gameObject);
            return;
        }
    }
}

[Serializable]
public class AudioEventData
{
    public string name;
    public AK.Wwise.Event audioEvent;
}
