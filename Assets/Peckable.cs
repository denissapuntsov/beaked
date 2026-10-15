using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class Peckable : ChainableAction
{
    private Interactable _interactable;
    public UnityEvent<ChainableAction> onStart;
    public float timeToReact;
    public UnityEvent<ChainableAction> onPeck;
    public UnityEvent<ChainableAction> onFinish;
    
    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _interactable.onPeck.AddListener(OnPeck);
    }

    private void OnPeck(ChainableAction parent)
    {
        parent.AddAction(this);
        onStart?.Invoke(this);
        Invoke(nameof(React), timeToReact);
    }

    public void React()
    {
        onFinish.Invoke(this);
        if (onPeck.GetPersistentEventCount() < 1)
        {
            Complete();
        }
        else
        {
            onPeck?.Invoke(this);
        }
    }

}
