using System;
using LevelStructure;
using UnityEngine;

[RequireComponent(typeof(Listener))]
public class Pipe : ChainableAction
{
    [SerializeField] private Pipe pairedPipe;
    
    [SerializeField] private LocalForward localForward;
    
    [SerializeField] private string command;
    
    private TileGridController _gridController;
    private GridElement _gridElement;
    private SoundItem _sound;
    
    private Listener _listener;

    private void Start()
    {
        _gridElement = GetComponent<GridElement>();
        _gridController = FindAnyObjectByType<TileGridController>();
        _listener = GetComponent<Listener>();
        _listener.onAnySound.AddListener(pairedPipe.TransmitSound);
    }

    private void TransmitSound(ChainableAction parent, SoundItem sound)
    {
        Debug.Log(parent.gameObject.name + " - " + sound.name);
        _sound = sound;
        parent.AddAction(this);

        var target = _gridController.GetElementFromCell(GetCellInFront());
        Debug.Log("target is: " + (target ? target.gameObject.name : "nothing"));
        if (!target || !target.GetComponent<Listener>())
        {
            Complete();
            return;
        }
        target.GetComponent<Commandable>()?.SetCommand(command);
        target.GetComponent<Listener>().Trigger(this, _sound);
        Complete();
    }

    private Cell GetCellInFront()
    {
        var cell = _gridElement.cells[0];
        var front = localForward switch
        {
            LocalForward.PlusX => new Vector3(1, 0, 0),
            LocalForward.MinusX => new Vector3(-1, 0, 0),
            LocalForward.PlusZ => new Vector3(0, 0, 1),
            LocalForward.MinusZ => new Vector3(0, 0, -1),
            LocalForward.PlusY => new Vector3(0, 1, 0),
            LocalForward.MinusY => new Vector3(0, -1, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return _gridController.TryGetCellAtLocation(cell.GridPosition + front);
    }
}

public enum LocalForward
{
    PlusX,
    MinusX,
    PlusZ,
    MinusZ,
    PlusY,
    MinusY
}
