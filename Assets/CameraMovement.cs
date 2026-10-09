using System;
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Tween _move;
    private Vector3 _startingPosition;
    
    private void Awake()
    {
        _startingPosition = transform.position;
        transform.position = _startingPosition - new Vector3(0, 0.05f, 0);
        _move = transform.DOMove(_startingPosition + new Vector3(0, 0.05f, 0), 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
