using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Interactable))]
public class BasicGridElementControls : GridElementControls
{
    [SerializeField] private Transform bodyPivot;

    [Header("Reaction Vectors")] 
    [SerializeField] private Vector3 westMovement = new Vector3(0, -1, 0);
    [SerializeField] private Vector3 southMovement = new Vector3(-1, 0, 0);
    [SerializeField] private Vector3 eastMovement = new Vector3(0, 1, 0);
    [SerializeField] private Vector3 northMovement = new Vector3(1, 0, 0);
    
    private Animator _animator;
    private Interactable _interactable;
    private ParticleSystem _particles;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _interactable = GetComponent<Interactable>();
        _particles = GetComponentInChildren<ParticleSystem>();
        
        _interactable.onPeck.AddListener(ReactToPeck);
    }

    public override void ReactToPeck(ChainableAction parent)
    {
        parent.AddAction(this);
        _particles?.Play();
        
        if (_animator)
        {
            _animator.enabled = false;
        }
        
        var turnDirection = Data.orientation switch
        {
            Orientation.West => westMovement,
            Orientation.South => southMovement,
            Orientation.East => eastMovement,
            Orientation.North => northMovement,
            _ => throw new ArgumentOutOfRangeException()
        };

        var randomFactor = Random.Range(0.5f, 1.2f);
        var turnVector = turnDirection * (randomFactor * 5);
        
        bodyPivot.DOBlendableLocalRotateBy(turnVector, 0.2f * randomFactor)
            .OnComplete(() =>
            {
                bodyPivot
                    .DOBlendableLocalRotateBy(-turnVector, 0.4f * randomFactor)
                    .OnComplete(() =>
                    {
                        if (_animator)
                        {
                            _animator.enabled = true;
                        }

                        Complete();
                    });
            });
    }
}
