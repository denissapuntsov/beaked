using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Detector))]
public class Speaker : MonoBehaviour
{
    public bool isSpeaking = true;
    
    public UnityEvent onListen;
    
    private Player _player;

    [SerializeField] private SoundItem sound;

    private List<Tile> _neighbors;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    public void Listen()
    {
        if (!isSpeaking) return;
        _player.currentSound = sound;
        onListen.Invoke();
    }
}
