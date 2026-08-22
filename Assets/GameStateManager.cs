using System;
using LevelStructure;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    [SerializeField] private TileGridController gridController;
    
    private PlayerInputActions _actions;
    private GlobalStateMachine _globalStateMachine;
    
    private void Awake()
    {
        _actions = new PlayerInputActions();
        _globalStateMachine = new GlobalStateMachine(_actions, data, gridController);
    }

    private void Start()
    {
        _globalStateMachine.Enter();
    }
}
