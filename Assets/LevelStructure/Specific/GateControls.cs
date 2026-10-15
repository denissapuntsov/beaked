using System;
using DG.Tweening;
using LevelStructure;
using UnityEngine;
using UnityEngine.Events;

public class GateControls : ChainableAction
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private GridElement invisibleWallClosed, invisibleWallOpen;
    private PlayerData _data;
    
    [SerializeField] private UnityEvent onOpen, onClose;

    private bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        private set 
        {
            _isOpen = value;
            invisibleWallClosed.SetSolid(!_isOpen);
            invisibleWallOpen.SetSolid(_isOpen);
            armTransform.localScale = new Vector3(armTransform.localScale.x, _isOpen ? 0.49f : 0.573f, armTransform.localScale.z);
        }
    }

    private void Start()
    {
        _data = FindAnyObjectByType<PlayerData>();
    }

    public void Toggle(ChainableAction source)
    {
        if (!IsOpen)
        {
            Open(source);
        }
        else
        {
            Close(source);
        }
    }

    private void Open(ChainableAction source)
    {
        if (IsOpen) return;
        onOpen?.Invoke();
        source.AddAction(this);
        armTransform
            .DOScaleY(0.5f, 1f);
        armTransform
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(90, 0, 0), 1f)
            .OnComplete(() =>
            {
                IsOpen = true;
                Complete();
            });
    }

    private void Close(ChainableAction source)
    {
        foreach (var cell in invisibleWallClosed.cells)
        {
            if (CheckCell(cell.GridPosition)) return;
        }
        if (!IsOpen) return;

        onClose?.Invoke();
        source.AddAction(this);
        armTransform
            .DOScaleY(0.573f, 1f);
        armTransform
            .DORotate(armTransform.rotation.eulerAngles + new Vector3(-90, 0, 0), 1f)
            .OnComplete(() =>
            {
                IsOpen = false;
                Complete();
            });
    }

    private bool CheckCell(Vector3 cellPosition)
    {
        return _data?.CurrentCell.GridPosition == cellPosition - new Vector3(0, 0.5f, 0);
    }
}
