/*
using System;
using DG.Tweening;
using NewTiles;
using UnityEngine;

public class PlayerEnterAnimator : MonoBehaviour
{
    public AnimationCurve cameraCurve, zoomCurve;
    
    Sequence _introSequence;
    
    private void Start()
    {
        var startingPosition = transform.position;
        transform.position += new Vector3(0, 40, 0);
        PlayerData.Instance.AnimationState = PlayerAnimationState.Intro;
        
        _introSequence = DOTween.Sequence();
        _introSequence
            .AppendInterval(2f)
            .Append(transform.DOMove(startingPosition, 0.8f))
            .Append(transform.DOPunchPosition(new Vector3(0, 0.5f, 0), 0.5f, 2, 2))
            .JoinCallback(() =>
            {
                PlayerData.Instance.AnimationState = PlayerAnimationState.Idle;
            })
            .AppendCallback(() =>
            {
                Camera.main.transform.DOMove(new Vector3(-7.42999983f,12.2799997f,-6.44097137f), 0.5f).SetEase(cameraCurve)
                    .OnComplete(() =>
                    {
                        Camera.main.DOOrthoSize(1f, 0.5f).SetEase(zoomCurve);
                    });
            });
    }
}
*/
