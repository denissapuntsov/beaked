using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Speaker : MonoBehaviour
{
    public UnityEvent onListen;
    
    [SerializeField] private Tile tile;
    private Player _player;
    
    public Tile Tile
    {
        get =>  tile;
        set => tile = value;
    }

    [SerializeField] private SoundItem sound;

    public int range = 1;

    private List<Tile> _neighbors;

    private void Start()
    {
        Setup();
        _player = FindAnyObjectByType<Player>();
        _player.onTileChange.AddListener(CheckPlayerTile);
    }

    private void Setup()
    {
        _neighbors = AStar.instance.GetNeighborsInRange(tile, range);
    }

    private void CheckPlayerTile(Tile playerTile)
    {
        if (_neighbors.Contains(playerTile))
        {
            _player.speakersInRange.Add(this);
        }
        else
        {
            _player.speakersInRange.Remove(this);
        }
    }

    public void Listen()
    {
        _player.currentSound = sound;
        onListen.Invoke();
    }

    private void OnDisable()
    {
        _player?.onTileChange.RemoveListener(CheckPlayerTile);
    }
}
