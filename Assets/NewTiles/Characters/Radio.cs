/*
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Speaker))]
public class Radio : MonoBehaviour
{
    private Speaker _speaker;
    
    [SerializeField] List<SoundItem> sounds = new List<SoundItem>();
    private int _soundIndex = 0;

    private void Start()
    {
        _speaker = GetComponent<Speaker>();
    }

    public void SwitchToNextSound()
    {
        _soundIndex = (_soundIndex + 1) == sounds.Count ? 0 : _soundIndex + 1;
        _speaker.Sound = sounds[_soundIndex];
    }
}
*/
