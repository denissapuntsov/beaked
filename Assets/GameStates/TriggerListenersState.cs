using System.Collections.Generic;
using NewTiles;
using UnityEngine;

public class TriggerListenersState : GameState
{
    public Listener[] Listeners = new Listener[4];
    
    public TriggerListenersState() : base()
    {
        
    }

    public override void Enter()
    {
        foreach (var listener in Listeners)
        {
            if (!listener) continue;
            if (listener.CheckForSound(PlayerData.Instance.currentSound))
            {
                Debug.Log(listener.gameObject);
                listener.Trigger(PlayerData.Instance.currentSound);
            }
        }

        Complete();
    }

    public override void Exit()
    {
        Listeners = new Listener[4];
    }
}
