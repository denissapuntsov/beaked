using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Detector))]
public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;

    private ActionPopup _popup;

    private void Start()
    {
        _popup = GetComponentInChildren<ActionPopup>();
    }

    public void Use()
    {
        Debug.Log($"Interacted with {gameObject.name}");
        onInteract.Invoke();
    }
}
