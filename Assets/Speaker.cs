using System;
using System.Collections.Generic;
using LevelStructure;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class Speaker : ChainableAction
{
    private List<Listener> _neighboringListeners;
    private GridElement _element;
    private TileGridController _gridController;
    private PlayerData _playerData;
    
    [SerializeField] private SoundItem sound;
    public Action<GridElement> OnSoundStarted, OnSoundComplete;

    private void Start()
    {
        _element = GetComponent<GridElement>();
        _gridController = FindAnyObjectByType<TileGridController>();
        _playerData = FindAnyObjectByType<PlayerData>();
    }

    public void Speak(ChainableAction source)
    {
        source.AddAction(this);
        
        var neighboringCells = GetNeighboringCells();
        if (CheckForPlayer(neighboringCells))
        {
            _playerData.currentSound = sound;
        }
        
        _neighboringListeners = FindListeners(neighboringCells);
        
        if (_neighboringListeners == null || _neighboringListeners.Count == 0)
        {
            Debug.Log("no listeners");
            Complete();
        }
        
        SoundManager.instance.PlaySound(sound, gameObject, HandleSoundComplete);
    }

    private bool CheckForPlayer(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (_gridController.GetElementFromCell(cell).GetComponent<PlayerData>())
            {
                return true;
            }
        }

        return false;
    }

    private void HandleSoundComplete(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
    {
        if (_neighboringListeners.Count == 0) return;
        TriggerReactionsInNeighbors();
    }

    private List<Cell> GetNeighboringCells()
    {
        var offsets = new Vector3[]
        {
            new (0, 0, 1),
            new (0, 0, -1),
            new (1, 0, 0),
            new (-1, 0, 0)
        };

        var neighboringCells = new List<Cell>();
        var cells = _element.cells;
        foreach (var cell in cells)
        {
            foreach (var offset in offsets)
            {
                var neighbor = _gridController.TryGetCellAtLocation(cell.GridPosition + offset);
                if (neighbor == null || cells.Contains(neighbor))
                {
                    continue;
                }
                neighboringCells.Add(neighbor);
            }
        }

        return neighboringCells;
    }
    
    private List<Listener> FindListeners(List<Cell> cellsAroundElement)
    {
        var listeners = new List<Listener>();

        foreach (var cell in cellsAroundElement)
        {
            var listener = _gridController.GetElementFromCell(cell)?.GetComponent<Listener>();
            if (listener && !listeners.Contains(listener))
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
