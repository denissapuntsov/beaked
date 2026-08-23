using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour, IChainableAction
{
    [SerializeField] private float speed = 1;
    public Action<IChainableAction> OnComplete { get; set; }
    
    public Dictionary<IChainableAction, bool> ActionsCompleteMap { get; set; } = new();

    public void AddReaction(IChainableAction reaction) { }

    public void Move(IChainableAction source)
    {
        source.AddReaction(this);
        transform.DOJump(
            transform.position, 
            0.5f,
            1,
            1 / speed).onComplete += () => OnComplete?.Invoke(this);
    }

}
