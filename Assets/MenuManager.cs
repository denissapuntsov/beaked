using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton, credits, creditsButton, backButton, mainGroup;

    private MenuType _selection;

    [SerializeField] private UnityEvent onSwitchSelection;
    
    public MenuType Selection
    {
        get => _selection;
        set
        {
            _selection = value;
            switch (value)
            {
                case MenuType.Play:
                    playButton.SetActive(true);
                    creditsButton.SetActive(false);
                    break;
                case MenuType.Credits:
                    creditsButton.SetActive(true);
                    playButton.SetActive(false);
                    break;
                case MenuType.Main:
                    creditsButton.SetActive(false);
                    playButton.SetActive(false);
                    break;
            }
        }
    }

    public MenuType currentMenu;

    private void Awake()
    {
        currentMenu = MenuType.Main;
        Selection = MenuType.Play;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SwitchSelection();
        }
    }

    public void SwitchSelection()
    {
        Debug.Log("switching selection");
        if (currentMenu == MenuType.Main)
        {
            onSwitchSelection?.Invoke();
            Selection = Selection == MenuType.Play ? MenuType.Credits : MenuType.Play;
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        SwitchMenus();
    }

    public void SwitchMenus()
    {
        onSwitchSelection?.Invoke();
        if (currentMenu == MenuType.Main)
        {
            if (Selection == MenuType.Credits)
            {
                mainGroup.SetActive(false);
                credits.SetActive(true);
                Selection = MenuType.Main;
                currentMenu = MenuType.Credits;
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        else if (currentMenu == MenuType.Credits)
        {
            mainGroup.SetActive(true);
            credits.SetActive(false);
            currentMenu = MenuType.Main;
            Selection = MenuType.Credits;
        }
    }
}

public enum MenuType
{
    Main,
    Credits,
    Play
}
