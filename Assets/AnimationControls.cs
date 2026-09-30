using UnityEngine;

public class AnimationControls : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayAnimation(string value)
    {
        if (!animator.HasState(0, Animator.StringToHash(value.ToString())))
        {
            Debug.Log("No state found");
            return;
        }
        Debug.Log(value);
        animator.Play(Animator.StringToHash(value.ToString()), 0, 0);
    }
    
    public void StopAnimation() => PlayAnimation("Idle");
}
