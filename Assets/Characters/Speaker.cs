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
        _audioSource.clip = sound.clip;
        _audioSource.Play();
        _popup?.Display(sound);
        _detector.currentState = NPCState.Speaking;
        if (!sound.clip) return;
        StartCoroutine(WaitUntilEndOfAudioFile());
    }

    public IEnumerator WaitUntilEndOfAudioFile()
    {
        var waitForClipRemainingTime = new WaitForSeconds(_audioSource.GetClipRemainingTime());
        yield return waitForClipRemainingTime;
        onFinishSpeaking?.Invoke();
        Debug.Log("Finished speaking");
        _popup?.Hide();
        _detector.currentState = NPCState.Idle;
    }
}
