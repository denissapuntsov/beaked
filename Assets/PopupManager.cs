using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using LevelStructure;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private Transform gameplayGroup;
    [SerializeField] private GameObject speechPopupPrefab, alertPopupPrefab;
    private PlayerData _playerData;
    private Camera _mainCamera;
    [SerializeField] private List<GridElement> elements = new List<GridElement>();
    private Dictionary<GridElement, GameObject> _speechPopups = new Dictionary<GridElement, GameObject>();
    private Dictionary<GridElement, GameObject> _alertPopups = new Dictionary<GridElement, GameObject>();

    public static PopupManager instance;

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

        _playerData = FindAnyObjectByType<PlayerData>();
        _mainCamera = Camera.main;
        
        
        SceneManager.sceneLoaded += RegisterElements;
    }

    private void RegisterElements(Scene scene, LoadSceneMode mode)
    {
        elements.Clear();

        RegisterListeners();
        RegisterSpeakers();
        RegisterPlayer();
    }

    private void RegisterPlayer()
    {
        var playerElement = _playerData?.GetComponent<GridElement>();
        elements.Add(playerElement);
    }

    private void RegisterSpeakers()
    {
        foreach (var speaker in FindObjectsByType<Speaker>())
        {
            var newElement = speaker.GetComponent<GridElement>();
            if (!elements.Contains(newElement)) 
            {
                elements.Add(newElement);
            }
            
            var newPopup = Instantiate(speechPopupPrefab, gameplayGroup);
            _speechPopups.TryAdd(newElement, newPopup);
            
            speaker.OnSoundStarted.AddListener(ShowSpeechPopup);
            speaker.OnSoundComplete.AddListener(HideSpeechPopup);
            
            newPopup.SetActive(false);
        }
    }

    private void RegisterListeners()
    {
        foreach (var listener in FindObjectsByType<Listener>())
        {
            var newElement = listener.GetComponent<GridElement>();
            if (!elements.Contains(newElement))
            {
                elements.Add(newElement);
            }

            var newPopup = Instantiate(alertPopupPrefab, gameplayGroup);
            _alertPopups.TryAdd(newElement, newPopup);

            listener.onListeningStarted.AddListener(ShowAlertPopup);
            listener.onListeningComplete.AddListener(HideAlertPopup);
            
            newPopup.SetActive(false);
        }
    }

    private void ShowSpeechPopup(GridElement element)
    {
        _speechPopups[element].SetActive(true);
    }

    private void HideSpeechPopup(GridElement element)
    {
        _speechPopups[element].SetActive(false);
    }

    private void ShowAlertPopup(GridElement element)
    {
        _alertPopups[element].SetActive(true);
    }

    private void HideAlertPopup(GridElement element)
    {
        _alertPopups[element].SetActive(false);
    }

    private void Update()
    {
        if (!_mainCamera) return;
        foreach (var (element, popup) in _speechPopups)
        {
            popup.transform.position = 
                _mainCamera.WorldToScreenPoint(
                    element.transform.position + 
                    element.transform.up * (element.GridSize.y + 0.5f));
        }

        foreach (var (element, popup) in _alertPopups)
        {
            popup.transform.position = 
                _mainCamera.WorldToScreenPoint(
                    element.transform.position + 
                    element.transform.up * (element.GridSize.y + 0.5f));
        }
    }
}