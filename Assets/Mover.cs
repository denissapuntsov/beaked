using System;
using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour, IReactionType
{
    public Action<IReactionType> OnComplete { get; set; }

    public void Move(Listener source)
    {
        source.AddReaction(this);
        transform.DOJump(
            transform.position, 
            0.5f,
            1,
            1).onComplete += () => OnComplete?.Invoke(this);
    }

}
