using System;
using DG.Tweening;
using LevelStructure;
using UnityEngine;

public class Gate : ChainableAction
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private GridElement invisibleWall;

    private bool _isOpen;
    private GridElement _gridElement;
    private bool IsOpen
    {
        get => _isOpen;
        set 
        {
            _isOpen = value;
            invisibleWall.gameObject.SetActive(!_isOpen);
        }
    }

    private void Start()
    {
        _gridElement = GetComponent<GridElement>();
    }

    public void Open(ChainableAction source)
    {
        if (IsOpen) return;
        source.AddAction(this);
        armTransform
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(0, 0, -90), 0.5f)
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
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(0, 0, 90), 0.5f)
            .OnComplete(() =>
            {
                IsOpen = false;
                Complete();
            });
    }
}
