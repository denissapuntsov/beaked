using System;
using System.Collections.Generic;
using System.Linq;
using NewTiles;
using UnityEngine;

public class TriggerListenerState : GameState
{
    public Listener Listener;
    private PlayerData _data;
    
    public TriggerListenerState(PlayerData data) : base()
    {
        _data = data;
    }

    public override void Enter()
    {
        Listener.OnComplete = HandleListenerComplete;
        Listener.Trigger(_data.currentSound);
    }

    private void HandleListenerComplete(ChainableAction listener)
    {
        Complete();
    }
}
