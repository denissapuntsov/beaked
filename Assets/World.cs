using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class World : ChainableAction
{
    [SerializeField] private List<GameObject> islands;
    [SerializeField] private int speed = 20;
    private int _currentIslandIndex = 0;
    private Action onSetupAnimationComplete, onBreakDownAnimationComplete;

    private Sequence _setup, _breakdown;

    public AnimationCurve worldEase;

    public void Setup(Action onComplete)
    {
        onSetupAnimationComplete += onComplete;
        
        foreach (var island in islands)
        {
            island.transform.position -= new Vector3(0, 20, 0);
        }

        MoveNextIslandUp();
    }

    private void MoveNextIslandUp()
    {
        if (_currentIslandIndex >= islands.Count)
        {
            onSetupAnimationComplete?.Invoke();
            return;
        }
        var island = islands[_currentIslandIndex];
        _currentIslandIndex++;
        _setup = DOTween.Sequence();

        var stretch = island.transform.DOScaleY(1.2f, 0.2f);
        var squish = island.transform.DOScaleY(1f, 0.2f);
        var moveUp = island.transform.DOMoveY(20, 1)
            .SetEase(worldEase)
            .SetRelative();

        _setup
            .Append(stretch)
            .Join(moveUp)
            .Insert(0.55f, squish)
            .InsertCallback(0.5f, MoveNextIslandUp);
    }

    public void Breakdown(Action onComplete)
    {
        onBreakDownAnimationComplete += onComplete;
        _currentIslandIndex = 0;
        MoveNextIslandDown();
    }

    public void MoveNextIslandDown()
    {
        if (_currentIslandIndex >= islands.Count)
        {
            onBreakDownAnimationComplete?.Invoke();
            return;
        }
        var island = islands[_currentIslandIndex];
        _currentIslandIndex++;
        _breakdown = DOTween.Sequence();
        
        var stretch = island.transform.DOScaleY(1.2f, 0.2f);
        var squish = island.transform.DOScaleY(1f, 0.2f);
        var moveDown = island.transform.DOMoveY(-20, 1)
            .SetEase(worldEase)
            .SetRelative();

        _breakdown
            .Append(stretch)
            .Join(moveDown)
            .Insert(0.55f, squish)
            .InsertCallback(0.5f, MoveNextIslandDown);
    }
}
