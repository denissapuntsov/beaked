using System;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup peck, speak;

    private bool _isPecking = false;

    public bool IsPecking
    {
        get => _isPecking;
        set
        {
            _isPecking = value;
            peck.alpha = value ? 0.5f : 1f;
        }
    }
    
    private bool _isSpeaking = false;

    public bool IsSpeaking
    {
        get => _isSpeaking;
        set
        {
            _isSpeaking = value;
            speak.alpha = value ? 0.5f : 1f;
        }
    }

    public void SetVisibility(bool doPeck, bool doSpeak)
    {
        IsPecking = doPeck;
        IsSpeaking = doSpeak;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
