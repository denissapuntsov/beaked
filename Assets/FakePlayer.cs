using UnityEngine;
using UnityEngine.Events;

public class FakePlayer : MonoBehaviour
{
    public UnityEvent onAnimationEnd;
    
    public void Complete()
    {
        onAnimationEnd.Invoke();
    }
}
