using System;
using DG.Tweening;
using LevelStructure;
using UnityEngine;

public class GateControls : ChainableAction
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private GridElement invisibleWallClosed, invisibleWallOpen;
    private PlayerData _data;

    private bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        private set 
        {
            _isOpen = value;
            invisibleWallClosed.SetSolid(!_isOpen);
            invisibleWallOpen.SetSolid(_isOpen);
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
        foreach (var cell in invisibleWallClosed.cells)
        {
            if (CheckCell(cell.GridPosition)) return;
        }
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

    private bool CheckCell(Vector3 cellPosition)
    {
        return _data?.CurrentCell.GridPosition == cellPosition - new Vector3(0, 0.5f, 0);
    }
}
