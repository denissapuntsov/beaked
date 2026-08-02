using System;
using System.Collections.Generic;
using NewTiles;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    
    [SerializeField] private GameObject popupPrefab;
    
    private Dictionary<GameObject, Popup> _popups = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void ShowPopup(GameObject parent, string text)
    {
        GameObject newPopup;
        if (!_popups.TryGetValue(parent, out var popupComponent))
        {
            newPopup = Instantiate(popupPrefab.gameObject, transform, true);
            _popups.Add(parent, newPopup.GetComponent<Popup>());
        }
        else
        {
            newPopup = popupComponent.gameObject;
        }
        
        var rectTransform = newPopup.GetComponent<RectTransform>();
        var screenPointPosition = Camera.main.WorldToScreenPoint(parent.transform.position);
            
        rectTransform.position = screenPointPosition + new Vector3(0, Screen.height * 0.1f, 0);
        rectTransform.localScale = Vector3.one;
        
        _popups[parent].SetActive(true);
        _popups[parent].Text = text;
    }

    public void HidePopup(GameObject parent)
    {
        if (!_popups.ContainsKey(parent)) return;
        _popups[parent].gameObject.SetActive(false);
    }
}
