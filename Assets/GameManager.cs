using System;
using LevelStructure;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] [InspectorName("Skip Setup animation?")] private bool isSetupSkipped = false;
    private PlayerData _data;
    private TileGridController _gridController;
    private MenuCanvas _menuCanvas;
    
    public PlayerInputActions Actions { get; private set; }
    private GlobalStateMachine _globalStateMachine;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        Actions = new PlayerInputActions();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _gridController = FindAnyObjectByType<TileGridController>();
        _data = FindAnyObjectByType<PlayerData>();
        _menuCanvas = FindAnyObjectByType<MenuCanvas>();
        _globalStateMachine = new GlobalStateMachine(Actions, _data, _gridController, isSetupSkipped, _menuCanvas);
        _globalStateMachine.Enter();
    }

    public void CompleteLevel()
    {
        _globalStateMachine.ChangeState(_globalStateMachine.LevelCompleteStateMachine);
        _globalStateMachine.Exit();
    }
}
