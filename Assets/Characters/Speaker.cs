using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Detector))] [RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    public bool playOnStart = true;
    
    public UnityEvent onListen, onFinishSpeaking;
    
    private Player _player;
    
    private AudioSource _audioSource;

    private ActionPopup _popup;

    private Detector _detector;

    [SerializeField] private SoundItem sound;

    public SoundItem Sound
    {
        get => sound;
        set
        {
            sound = value;
            _popup?.Display(sound);
        }
    }

    private List<Tile> _neighbors;

    private void Start()
    {
        _popup = GetComponentInChildren<ActionPopup>();
        _player = FindAnyObjectByType<Player>();
        _audioSource = GetComponent<AudioSource>();
        _detector = GetComponent<Detector>();
        if (!playOnStart) return;
        Speak();
    }

    public void Listen()
    {
        if (_detector.currentState != NPCState.Speaking) return;
        _player.currentSound = sound;
        onListen?.Invoke();
    }

    public void Speak()
    {
        if (_detector.currentState == NPCState.Speaking) return;
        if (!sound) return;
        _popup?.Display(sound);
        _detector.currentState = NPCState.Speaking;
        sound.audioEvent.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, FinishSpeaking);
    }

    private void FinishSpeaking(object in_cookie, AkCallbackType in_type, object in_info)
    {
        onFinishSpeaking?.Invoke();
        _popup?.Hide();
        _detector.currentState = NPCState.Idle;
    }
}
