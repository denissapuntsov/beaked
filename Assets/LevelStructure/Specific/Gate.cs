using System;
using DG.Tweening;
using LevelStructure;
using UnityEngine;

public class Gate : ChainableAction
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private GridElement invisibleWallClosed, invisibleWallOpen;

    private bool _isOpen;
    private GridElement _gridElement;
    private bool IsOpen
    {
        get => _isOpen;
        set 
        {
            _isOpen = value;
            invisibleWallClosed.SetSolid(!_isOpen);
            invisibleWallOpen.SetSolid(_isOpen);
        }
    }

    private void Start()
    {
        _gridElement = GetComponent<GridElement>();
        IsOpen = false;
    }

    public void Open(ChainableAction source)
    {
        if (IsOpen) return;
        source.AddAction(this);
        armTransform
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(180, 0, 0), 1f)
            .OnComplete(() =>
            {
                IsOpen = true;
                Complete();
            });
    }

    public void Close(ChainableAction source)
    {
        if (!IsOpen) return;
        source.AddAction(this);
        armTransform
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(-180, 0, 0), 1f)
            .OnComplete(() =>
            {
                IsOpen = false;
                Complete();
            });
    }
}
