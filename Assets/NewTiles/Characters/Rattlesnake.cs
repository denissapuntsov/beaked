/*
using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Detector))] [RequireComponent(typeof(Speaker))]
public class Rattlesnake : MonoBehaviour
{
    private Detector _detector;
    private Speaker _speaker;
    private void Start()
    {
        transform.localScale = new Vector3(1, 0, 1);
        _detector = GetComponent<Detector>();
        _speaker = GetComponent<Speaker>();

        _detector.onPlayerDetected.AddListener(Appear);
        _detector.onPlayerLost.AddListener(Disappear);
        
        _speaker.onListen.AddListener(() =>
        {
            Disappear();
            _detector.onPlayerDetected.RemoveListener(Appear);
            _detector.onPlayerLost.RemoveListener(Disappear);
        });
    }

    public void Appear()
    {
        _detector.currentState = NPCState.Changing;
        transform.DOScaleY(1, 0.2f).OnComplete(() => _detector.currentState = NPCState.Idle);
    }

    public void Disappear()
    {
        _detector.currentState = NPCState.Changing;
        transform.DOScaleY(0, 0.2f).OnComplete(() => _detector.currentState = NPCState.Idle);
    }

    private void OnDisable()
    {
        _detector.onPlayerDetected.RemoveListener(Appear);
        _detector.onPlayerLost.RemoveListener(Disappear);
    }
}
*/
