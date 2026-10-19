using System;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Listener))]
public class Pipe : ChainableAction
{
    [SerializeField] private Pipe pairedPipe;

    [SerializeField] private Transform forward;
    
    [SerializeField] private string command;
    
    private TileGridController _gridController;
    private GridElement _gridElement;
    private SoundItem _sound;
    private Listener _target;

    [SerializeField] private UnityEvent onSoundReceived, onSoundEnded;
    
    private Listener _listener;

    private void Start()
    {
        _gridElement = GetComponent<GridElement>();
        _gridController = FindAnyObjectByType<TileGridController>();
        _listener = GetComponent<Listener>();
        _listener.onAnySound.AddListener(pairedPipe.TransmitSound);
    }

    public void SetCommand(string newCommand)
    {
        command = newCommand;
    }

    private void TransmitSound(ChainableAction parent, SoundItem sound)
    {
        parent.AddAction(this);
        _target = GetListenerInFront();
        _sound = sound;
        onSoundReceived?.Invoke();
        SoundManager.instance.PlaySound(_sound, SoundManager.instance.gameObject, TriggerListener);
    }

    private void TriggerListener(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        onSoundEnded?.Invoke();
        if (!_target)
        {
            Complete();
            return;
        }

        _target.GetComponent<Commandable>().SetCommand(command);
        _target.Trigger(_sound);
        Complete();
    }

    /*private void TransmitSound(ChainableAction parent, SoundItem sound)
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
    }*/

    public Listener GetListenerInFront()
    {
        var forwardWorldPosition = new Vector3(
            Mathf.Round(forward.position.x * 10) / 10, 
            Mathf.Round(forward.position.y * 10) / 10, 
            Mathf.Round(forward.position.z * 10) / 10);
        
        var cellInFront = _gridController.TryGetCellAtLocation(forwardWorldPosition);
        var target = _gridController.GetElementFromCell(cellInFront);
        
        return target?.GetComponent<Listener>();
    }
}