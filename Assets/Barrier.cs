/*
using DG.Tweening;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class Barrier : MonoBehaviour
{
    [SerializeField] private float startRotationY, endRotationY;
    [SerializeField] private GameObject barrierChild;

    private Tween _open, _close;

    public UnityEvent onOpen, onClose;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _open = barrierChild.transform.DOLocalRotate(
                new(0, endRotationY, 0),
                2f)
            .SetEase(Ease.InOutElastic, 1.2f)
            .OnComplete(() => onOpen?.Invoke())
            .SetAutoKill(false)
            .Pause();

        _close = barrierChild.transform.DOLocalRotate(
                new(0, startRotationY, 0),
                1f)
            .SetEase(Ease.InOutBack, 1.2f)
            .OnComplete(() => onClose?.Invoke())
            .SetAutoKill(false)
            .Pause();
    }

    public void Open()
    {
        Debug.Log("Opening");
        _open.Restart();
        _open.Play();
    }

    public void OpenForPlayer(Obstacle obstacle)
    {
        barrierChild.transform.DOLocalRotate(
                new(0, endRotationY, 0),
                2f)
            .SetEase(Ease.InOutElastic, 1.2f)
            .OnComplete(() =>
            {
                //GetComponentInParent<Obstacle>().enabled = false;
                Debug.Log(obstacle.OccupiedTile.name);
                obstacle.UnblockTile();
            });
    }

    public void Close()
    {
        _close.Restart();
    }

}
*/
