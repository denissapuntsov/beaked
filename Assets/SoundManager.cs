using System;
using UnityEngine;
using LevelStructure;
using AK.Wwise;

public class SoundManager : MonoBehaviour
{
    private const string FootstepSwitch = "StepMaterial";
    private const string PeckSwitch = "PeckMaterial";
    
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

    public void SetFootstepMaterial(StepMaterial material)
    {
        AkUnitySoundEngine.SetSwitch(FootstepSwitch, material.ToString(), gameObject);
    }

    public void SetPeckMaterial(PeckMaterial material)
    {
        AkUnitySoundEngine.SetSwitch(PeckSwitch, material.ToString(), gameObject);
    } 

    public void PlaySound(SoundItem sound, GameObject source, AkCallbackManager.EventCallback onFinishedSpeaking)
    { 
        sound.audioEvent.Post(source, (uint)AkCallbackType.AK_EndOfEvent, onFinishedSpeaking);
    }
}
