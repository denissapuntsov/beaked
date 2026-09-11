using System;
using DG.Tweening;
using LevelStructure;
using UnityEngine;

public class GateControls : ChainableAction
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private GridElement invisibleWallClosed, invisibleWallOpen;

    private bool _isOpen;
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

    public void Open(ChainableAction source)
    {
        if (IsOpen) return;
        source.AddAction(this);
        armTransform
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(90, 0, 0), 1f)
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
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(-90, 0, 0), 1f)
            .OnComplete(() =>
            {
                IsOpen = false;
                Complete();
            });
    }
}
