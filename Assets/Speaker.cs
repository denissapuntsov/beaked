using System.Collections.Generic;
using LevelStructure;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class Speaker : ChainableAction
{
    [SerializeField] private Vector3 cellLocation;
    
    private List<Listener> _neighboringListeners;
    private GridElement _element;
    private TileGridController _gridController;
    
    [SerializeField] private SoundItem sound;

    private void Start()
    {
        _element = GetComponent<GridElement>();
        _gridController = FindAnyObjectByType<TileGridController>();
    }

    public void Speak(ChainableAction source)
    {
        source.AddAction(this);
        _neighboringListeners = FindListeners(GetNeighboringCells());
        
        if (_neighboringListeners == null || _neighboringListeners.Count == 0)
        {
            Debug.Log("no listeners");
            Complete();
            SoundManager.instance.PlaySound(sound, gameObject, OnSoundOver);
            return;
        }
        SoundManager.instance.PlaySound(sound, gameObject, HandleSoundComplete);
    }

    private void OnSoundOver(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        
    }

    private void HandleSoundComplete(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        TriggerReactionsInNeighbors();
    }

    private List<Cell> GetNeighboringCells()
    {
        return new List<Cell>
        {
            _gridController.TryGetCellAtLocation(cellLocation) // new Vector3(0, 3, 10)
        };
    }
    
    private List<Listener> FindListeners(List<Cell> cellsAroundElement)
    {
        var listeners = new List<Listener>();

        foreach (var cell in cellsAroundElement)
        {
            var listener = _gridController.GetElementFromCell(cell)?.GetComponent<Listener>();
            if (listener)
            {
                listeners.Add(listener);
            }
        }

        return listeners;
    }

    private void TriggerReactionsInNeighbors()
    {
        foreach (var listener in _neighboringListeners)
        {
            if (!listener) continue;
            if (listener.CheckForSound(sound))
            {
                listener.Trigger(this, sound);
                AddAction(listener);
            }
        }
    }
}
