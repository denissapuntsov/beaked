using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Detector))]
public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;

    public void Use()
    {
        onInteract.Invoke();
    }
}
