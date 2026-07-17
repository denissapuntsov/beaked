using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Page initialPage;
    private GameObject _currentSelection;
    
    private Page _activePage;

    public Page ActivePage
    {
        get => _activePage;
        set
        {
            _activePage = value;
            //EventSystem.current.SetSelectedGameObject(value ? _activePage.Selectables[0].gameObject : null);
            //_currentSelection = EventSystem.current.currentSelectedGameObject;
        }
    }
    
    public static MenuManager instance;
    
    private Stack<Page> _stack;

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

        Setup();
        
        _stack = new Stack<Page>();
    }

    private void Setup()
    {
        foreach (var page in GetComponentsInChildren<Page>())
        {
            page.gameObject.SetActive(false);
        }
    }
    
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!ActivePage)
        {
            if (!initialPage) return;
            Time.timeScale = 0;
            Push(initialPage);
            return;
        }
        Pop();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!ActivePage || _currentSelection) return;
        _currentSelection.GetComponent<Selectable>().Select();
    }
    
    public void Push(Page page)
    {
        ActivePage?.gameObject.SetActive(false);
        if (!_stack.Contains(page))
        {
            _stack.Push(page);
        }
        ActivePage = page;
        ActivePage.gameObject.SetActive(true);
    }

    public void Pop()
    {
        var page = _stack.Pop();
        page.gameObject.SetActive(false);
        ActivePage = null;
        
        if (_stack.Count == 0)
        {
            Time.timeScale = 1;
            _stack = new Stack<Page>();
        }
        else
        {
            ActivePage = _stack.Peek();
        }
    }
}
