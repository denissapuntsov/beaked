using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    
    private PlayerInputActions _actions;
    private GameplayStateMachine _gameplayStateMachine;
    
    private void Awake()
    {
        _actions = new PlayerInputActions();
        _gameplayStateMachine = new GameplayStateMachine(_actions);
    }

    private void Start()
    {
        _gameplayStateMachine.Enter();
    }
}
