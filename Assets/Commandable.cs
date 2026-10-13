using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Commandable : ChainableAction
{
    [SerializeField] private List<Command> commands;
    private Dictionary<string, Command> _commands = new();

    private Command _currentCommand;

    private void Start()
    {
        foreach (var command in commands)
        {
            _commands.Add(command.key, command);
        }
    }

    public void SetCommand(string key)
    {
        _commands.TryGetValue(key, out _currentCommand);
    }

    public void ExecuteCurrentCommand(ChainableAction parent)
    {
        parent.AddAction(this);
        if (_currentCommand == null)
        {
            Complete();
            return;
        }
        _currentCommand.onCommandStarted.Invoke(this);
        Complete();
    }
}

[Serializable]
public class Command
{
    public string key;
    public UnityEvent<ChainableAction> onCommandStarted, onCommandEnded;
}
