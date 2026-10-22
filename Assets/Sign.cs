using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Sign : Interactable
{
    public UnityEvent onComplete;

    public UnityEvent onPecked;

    public void Complete()
    {
        onComplete?.Invoke();
    }
    
    public void NextLevel()
    {
        onPecked?.Invoke();
    }
}
