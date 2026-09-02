using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using LevelStructure;
using UnityEngine.UIElements;

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
    }

    public void RegisterPopup(GridElement origin)
    {
        var panel = origin.gameObject.GetComponentInChildren<PanelRenderer>();
        if (panel)
        {
            _elementToPopupMap.TryAdd(origin, panel.gameObject);
        }
    }

    public void OpenPopup(GridElement origin)
    {
        _elementToPopupMap.TryGetValue(origin, out var popup);
        if (!popup) return;
        
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