using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Sign : Interactable
{
    public UnityEvent onComplete;

    public void Complete()
    {
        onComplete?.Invoke();
    }
    
    public void CompleteLevel() => GameManager.instance.CompleteLevel();
    
    public void NextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
