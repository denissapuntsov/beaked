using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ActionPopup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frame;
    [SerializeField] private SpriteRenderer icon;

    private Sequence _show, _hide;

    private void Start()
    {
        frame.color = new Color(1f, 1f, 1f, 0f);
        icon.color = new Color(1f, 1f, 1f, 0f);
        
        SetupTweens();
    }

    private void SetupTweens()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        transform.localScale = Vector3.zero;
        
        _show =
            DOTween.Sequence()
                .Join(transform
                    .DOMoveY(transform.position.y + 0.5f, 0.2f)
                    .SetEase(Ease.InOutBounce))
                .Join(transform.DOScale(1f, 0.2f))
                .Join(frame.DOFade(1, 0.3f))
                .Join(icon.DOFade(1, 0.3f))
                .SetAutoKill(false);

        _show.Rewind();
    }

    public void Display(SoundItem soundItem)
    {
        icon.sprite = soundItem.sprite;
        _show.Rewind();
        _show.PlayForward();
    }

    public void Hide()
    {
        _show.PlayBackwards();
    }
}
