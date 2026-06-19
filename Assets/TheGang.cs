using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TheGang : MonoBehaviour
{
    [SerializeField] private List<SoundItem> sounds;

    private List<SoundItem> _order;

    private Dictionary<SoundItem, GameObject> _soundPersonPairs = new Dictionary<SoundItem, GameObject>();
    
    private bool _isBusy = false;
    
    private void Awake()
    {
        Setup();
        ScrambleOrder();
    }

    private void ScrambleOrder()
    {
        _order = new List<SoundItem>();
        foreach (var soundItem in sounds)
        {
            var nextSound = sounds[Random.Range(0, sounds.Count)];
            while (_order.Contains(nextSound))
            {
                nextSound = sounds[Random.Range(0, sounds.Count)];
            }
            _order.Add(nextSound);
        }
    }

    private void Setup()
    {
        SoundItem[] temp = new SoundItem[sounds.Count];
        sounds.CopyTo(temp);
        var tempList = temp.ToList();
        
        Detector[] detectors = GetComponentsInChildren<Detector>();
        
        foreach (var detector in detectors)
        {
            detector.onPlayerDetected.AddListener(Detect);
            detector.onPlayerLost.AddListener(LosePlayer);
            
            var sound = tempList[Random.Range(0, tempList.Count)];
            _soundPersonPairs.Add(sound, detector.gameObject);
            tempList.Remove(sound);
        }

        foreach (var pair in _soundPersonPairs)
        {
            Debug.Log($"{pair.Value} is afraid of {pair.Key}");
        };
        
        Listener[] listeners = GetComponentsInChildren<Listener>();
        
        foreach (var listener in listeners)
        {
            listener.onListen.AddListener(TryScare);
        }
    }

    private void Detect()
    {
        if (_isBusy) return;
        _isBusy = true;
        Debug.Log("Player detected!");
    }

    private void LosePlayer() => _isBusy = false;

    private void TryScare(SoundItem sound)
    {
        if (_order.Count < 1) return;
        if (sound != _order[0])
        {
            Debug.Log($"{sound} is wrong!");
            return;
        }

        _soundPersonPairs[sound].gameObject.SetActive(false);
        _order.RemoveAt(0);
    }
}
