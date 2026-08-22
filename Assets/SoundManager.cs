using System;
using UnityEngine;
using LevelStructure;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
        }
        else if (instance != this)
        { 
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundItem sound, GameObject source, AkCallbackManager.EventCallback onFinishedSpeaking)
    { 
        sound.audioEvent.Post(source, (uint)AkCallbackType.AK_EndOfEvent, onFinishedSpeaking);
    }
}
