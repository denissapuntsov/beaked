using UnityEngine;
using UnityEngine.SceneManagement;

public class Sign : Interactable
{
    public void CompleteLevel() => GameManager.instance.CompleteLevel();
    
    public void NextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
