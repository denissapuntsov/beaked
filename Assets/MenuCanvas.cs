using System;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup peck, speak;

    private bool _isPecking = false;

    public GameObject pauseGroup, gameplayGroup;
    public GameObject resume, exit;
    public GameObject activeButton;

    public static MenuCanvas instance;

    public bool isPaused = false;

    public void Awake()
    {
        instance = this;
        activeButton = resume;
        resume.gameObject.SetActive(true);
        exit.gameObject.SetActive(false);
    }
    
    public bool IsPecking
    {
        get => _isPecking;
        set
        {
            _isPecking = value;
            if (peck)
            {
                peck.alpha = value ? 1f : 0.5f;
            }
        }
    }
    
    private bool _isSpeaking = false;

    public bool IsSpeaking
    {
        get => _isSpeaking;
        set
        {
            _isSpeaking = value;
            if (speak)
            {
                speak.alpha = value ? 1f : 0.5f;
            }
        }
    }

    public void SetPause(bool newIsPaused)
    {
        pauseGroup.SetActive(isPaused);
        gameplayGroup.SetActive(!isPaused);
        activeButton = isPaused ? resume : null;
    }

    public void Navigate()
    {
        activeButton.SetActive(false);
        activeButton = activeButton == resume ? exit : resume;
        activeButton.SetActive(true);
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
