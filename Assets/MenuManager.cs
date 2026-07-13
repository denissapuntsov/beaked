using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Page initialPage;
    private Page _activePage;
    
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
        if (!_activePage)
        {
            if (!initialPage) return;
            Time.timeScale = 0;
            Push(initialPage);
        }
        else
        {
            Pop();
        }
    }

    public void Push(Page page)
    {
        page.gameObject.SetActive(true);
        _stack.Push(page);
        _activePage = page;
    }

    public void Pop()
    {
        var page = _stack.Pop();
        page.gameObject.SetActive(false);
        _activePage = null;
        
        if (_stack.Count < 1)
        {
            Time.timeScale = 1;
        }
        else
        {
            Push(_stack.Peek());
        }
    }
}
