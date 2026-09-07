using System;
using System.Collections.Generic;
using System.Linq;
using NewTiles;
using UnityEngine;

public class TriggerListenersState : GameState
{
    public Listener[] Listeners = new Listener[4];
    private PlayerData _data;
    private readonly Dictionary<ChainableAction, bool> _listenerCompletionMap = new();
    
    public TriggerListenersState(PlayerData data) : base()
    {
        _data = data;
    }

    public override void Enter()
    {
        foreach (var listener in Listeners)
        {
            if (!listener) continue;
            AddListener(listener);
            listener.Trigger(_data.currentSound);
        }
    }

    private void AddListener(ChainableAction listener)
    {
        _listenerCompletionMap.Add(listener, false);
        listener.OnComplete += MarkListenerAsComplete;
    }

    private void MarkListenerAsComplete(ChainableAction listener)
    {
        _listenerCompletionMap[listener] = true;
        CheckForCompletion();
    }

    private void CheckForCompletion()
    {
        if (_listenerCompletionMap.Values.Contains(false)) return;

        foreach (var listener in _listenerCompletionMap.Keys)
        {
            listener.OnComplete -= MarkListenerAsComplete;
        }
        _listenerCompletionMap.Clear();
        Complete();
    }

    public override void Exit()
    {
        Listeners = new Listener[4];
    }
}
