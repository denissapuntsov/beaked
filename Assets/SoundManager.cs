using System;
using UnityEngine;
using LevelStructure;
using AK.Wwise;
using Event = AK.Wwise.Event;

public class SoundManager : MonoBehaviour
{
    private const string FootstepSwitch = "StepMaterial";
    private const string PeckSwitch = "PeckMaterial";
    public static readonly string AssemblyEventName = "assembly";
    
    [SerializeField] private Event assemblyEvent;

    [SerializeField] private AK.Wwise.Event musicEvent;
    
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
        
        musicEvent.Post(gameObject);
    }

    public void SetFootstepMaterial(StepMaterial material)
    {
        AkUnitySoundEngine.SetSwitch(FootstepSwitch, material.ToString(), gameObject);
    }

    public void SetPeckMaterial(PeckMaterial material)
    {
        AkUnitySoundEngine.SetSwitch(PeckSwitch, material.ToString(), gameObject);
    }

    public void PlaySoundPipe(SoundItem sound, GameObject source, AkCallbackManager.EventCallback onFinishedSpeaking)
    {
        sound.pipeEvent.Post(source, (uint)AkCallbackType.AK_EndOfEvent, onFinishedSpeaking);
    }

    public void PlaySound(SoundItem sound, GameObject source, AkCallbackManager.EventCallback onFinishedSpeaking)
    { 
        sound.audioEvent.Post(source, (uint)AkCallbackType.AK_EndOfEvent, onFinishedSpeaking);
    }

    public void PlaySound(string constEventName)
    {
        switch (constEventName)
        {
            default:
                break;
            case var s when s == AssemblyEventName:
                assemblyEvent.Post(gameObject);
                break;
        }
    }

    public void SetMusic(MusicMode mode)
    {
        switch (mode)
        {
            case MusicMode.Chapter1:
                break;
            case MusicMode.Chapter2:
                break;
            case MusicMode.Chapter3:
                break;
        }
    }

    public void ResetAssembly()
    {
        AkUnitySoundEngine.PostEvent("Reset_Assembly", gameObject);
    }
}

public enum MusicMode
{
    Chapter1,
    Chapter2,
    Chapter3
}
