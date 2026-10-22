using UnityEngine;
using UnityEngine.Events;

public class FakePlayer : MonoBehaviour
{
    public UnityEvent onSetupAnimationEnd, onBreakdownAnimationStart, onBreakdownAnimationEnd;

    public void OnEnable()
    {
        transform.localRotation = GetComponentInParent<PlayerData>().meshAnimator.transform.localRotation;
    }
    
    public void StartBreakdown()
    {
        onBreakdownAnimationStart?.Invoke();
    }
    
    public void CompleteSetup()
    {
        onSetupAnimationEnd?.Invoke();
    }

    public void CompleteBreakdown()
    {
        onBreakdownAnimationEnd?.Invoke();
    }
}
