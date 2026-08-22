using System.Collections.Generic;
using System.Linq;
using NewTiles;
using UnityEngine;

public class TriggerListenersState : GameState
{
    public Listener[] Listeners = new Listener[4];
    private PlayerData _data;
    private Dictionary<Listener, bool> _listenerCompletionMap = new();
    
    public TriggerListenersState(PlayerData data) : base()
    {
        _data = data;
    }

    public override void Enter()
    {
        foreach (var listener in Listeners)
        {
            if (!listener) continue;
            if (listener.CheckForSound(_data.currentSound))
            {
                listener.Trigger(_data.currentSound);
                AddListener(listener);
            }
        }
    }

    private void AddListener(Listener listener)
    {
        _listenerCompletionMap.Add(listener, false);
        listener.OnComplete += MarkListenerAsComplete;
    }

    private void MarkListenerAsComplete(Listener listener)
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
