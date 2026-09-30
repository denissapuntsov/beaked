using System;
using DG.Tweening;
using UnityEngine;

public class PersonControls : GridElementControls
{
    [SerializeField] private Transform bodyPivot;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void ReactToPeck()
    {
        _animator.enabled = false;
        var turnDirection = Data.orientation switch
        {
            Orientation.West => new Vector3(0, -1, 0),
            Orientation.South => new Vector3(-1, 0, 0),
            Orientation.East => new Vector3(0, 1, 0),
            Orientation.North => new Vector3(1, 0, 0),
            _ => throw new ArgumentOutOfRangeException()
        };

        var turnVector = turnDirection * 5;
        Debug.Log(turnVector);
        bodyPivot.DOBlendableLocalRotateBy(turnVector, 0.2f)
            .OnComplete(() =>
            {
                bodyPivot
                    .DOBlendableLocalRotateBy(-turnVector, 0.4f)
                    .OnComplete(() =>
                    {
                        _animator.enabled = true;
                    });
            });
    }
}
