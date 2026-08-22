using System.Collections.Generic;
using NewTiles;
using UnityEngine;

public class TriggerListenersState : GameState
{
    public Listener[] Listeners = new Listener[4];
    private PlayerData _data;
    
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
                Debug.Log(listener.gameObject);
                listener.Trigger(_data.currentSound);
            }
        }

        Complete();
    }

    public override void Exit()
    {
        Listeners = new Listener[4];
    }
}
