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
    [SerializeField] private GameObject popupPrefab;
    private Dictionary<GridElement, GameObject> _elementToPopupMap = new();

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

        SceneManager.sceneLoaded += RegisterSpeakers;
    }

    private void RegisterSpeakers(Scene scene, LoadSceneMode mode)
    {
        foreach (var speaker in FindObjectsByType<Speaker>())
        {
            _elementToPopupMap.Clear();
            RegisterPopup(speaker.GetComponent<GridElement>());
            speaker.OnSoundStarted.AddListener(OpenPopup);
            speaker.OnSoundComplete.AddListener(ClosePopup);
        }
    }

    public void RegisterPopup(GridElement origin)
    {
        var panel = Instantiate(popupPrefab, origin.transform);
        panel.transform.localScale = Vector3.zero;
        if (panel)
        {
            _elementToPopupMap.TryAdd(origin, panel.gameObject);
        }
    }

    public void OpenPopup(GridElement origin)
    {
        _elementToPopupMap.TryGetValue(origin, out var popup);
        if (!popup) {return;}
        
        popup.transform.localPosition = Vector3.up * (origin.GridSize.y + 0.25f);
        
        popup.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 0.1f);
    }

    public void ClosePopup(GridElement origin)
    {
        _elementToPopupMap.TryGetValue(origin, out var popup);
        if (!popup) return;
        
        popup.transform.DOScale(Vector3.zero, 0.1f);
    }
}