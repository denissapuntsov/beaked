using UnityEngine;

public class Sign : Interactable
{
    public void CompleteLevel() => GameManager.instance.CompleteLevel();
}
