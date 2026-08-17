using System.Collections.Generic;
using UnityEngine;

public class ReactionStateMachine : GameState
{
    private List<GameObject> _listenersToTrigger = new List<GameObject>();
    
    public ReactionStateMachine(List<GameObject> listenersToTrigger) : base()
    {
        _listenersToTrigger = listenersToTrigger;
    }

    public override void Enter()
    {
        Debug.Log("Reaction step!");
        if (_listenersToTrigger == null || _listenersToTrigger.Count == 0)
        {
            Complete();
        }
    }
}
