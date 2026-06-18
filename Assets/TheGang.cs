using System;
using UnityEngine;

public class TheGang : MonoBehaviour
{
    private bool _isBusy = false;
    
    private void Awake()
    {
        foreach (var child in GetComponentsInChildren<Detector>())
        {
            child.onPlayerDetected.AddListener(Detect);
        }
    }

    public void Detect()
    {
        if (_isBusy) return;
        _isBusy = true;
        Debug.Log("Player detected!");
    }
}
