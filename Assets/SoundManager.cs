using System;
using UnityEngine;
using LevelStructure;
using AK.Wwise;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Event = AK.Wwise.Event;

public class SoundManager : MonoBehaviour
{
    private const string FootstepSwitch = "StepMaterial";
    private const string PeckSwitch = "PeckMaterial";
    public static readonly string AssemblyEventName = "assembly";
    
    //public AK.Wwise.Event 
    
    [SerializeField] private Event assemblyEvent;

    [SerializeField] private AK.Wwise.Event ch1, ch2, ch3, ch2Intro, ch3Intro, menu;
    
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
        
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += PlayMusicEvent;

        //PlaySound(StartMusic);
    }

    private void PlayMusicEvent(Scene scene, LoadSceneMode loadSceneMode)
    {
        AK.Wwise.Event musicEvent = new Event();
        switch (scene.name)
        {
            case "MainMenu":
                musicEvent = menu;
                break;
            case "Level 1-1":
                musicEvent = ch2;
                break;
            case "Level 2-1":
                musicEvent = ch3;
                break;
            case "Ch1Transition":
                musicEvent = ch1;
                break;
            case "Ch2Transition":
                musicEvent = ch3Intro;
                break;
            case "Ch3Transition":
                musicEvent = ch3Intro;
                break;
            case "FinalTransition":
                musicEvent = ch3Intro;
                break;
        }

        if (musicEvent.WwiseObjectReference != null)
        {
            AkUnitySoundEngine.StopAll();
        }
        
        musicEvent?.Post(instance.gameObject);
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

    public void PlaySoundBird(SoundItem sound, GameObject source, AkCallbackManager.EventCallback onFinishedSpeaking)
    {
        sound.playerEvent.Post(source, (uint)AkCallbackType.AK_EndOfEvent, onFinishedSpeaking);
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
        
    }

    public void ResetAssembly()
    {
        AkUnitySoundEngine.PostEvent("Reset_Assembly", gameObject);
    }
}

public enum MusicMode
{
    Ch1,
    Ch2,
    Ch3,
    Ch2Intro,
    Ch3Intro,
    End,
    Menu
}
