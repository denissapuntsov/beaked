using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingElement : MonoBehaviour
{
    [SerializeField] private List<MoveCommand> moveCommands = new List<MoveCommand>();

    private Dictionary<string, MoveCommand> _commands = new ();
    
    public void SetCommand(string key)
    {
        
    }
}

[Serializable]
public class MoveCommand
{
    public string key;
    public List<Vector3> directions;
    public UnityEvent onCommandStarted, onCommandEnded;
}