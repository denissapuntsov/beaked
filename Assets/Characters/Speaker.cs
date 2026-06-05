using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Detector))] [RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    public bool isSpeaking = false;
    
    public UnityEvent onListen, onFinishSpeaking;
    
    private Player _player;
    
    private AudioSource _audioSource;

    private ActionPopup _popup;

    [SerializeField] private SoundItem sound;

    private List<Tile> _neighbors;

    private void Start()
    {
        _popup = GetComponentInChildren<ActionPopup>();
        _player = FindAnyObjectByType<Player>();
        _audioSource = GetComponent<AudioSource>();
        Speak();
    }

    public void Listen()
    {
        if (!isSpeaking) return;
        _player.currentSound = sound;
        onListen?.Invoke();
    }

    public void Speak()
    {
        if (isSpeaking) return;
        _audioSource.clip = sound.clip;
        _audioSource.Play();
        _popup.Display(sound);
        isSpeaking = true;
        StartCoroutine(WaitUntilEndOfAudioFile());
    }

    public IEnumerator WaitUntilEndOfAudioFile()
    {
        var waitForClipRemainingTime = new WaitForSeconds(_audioSource.GetClipRemainingTime());
        yield return waitForClipRemainingTime;
        onFinishSpeaking?.Invoke();
        _popup.Hide();
        isSpeaking = false;
    }
}
