using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    public AnimationCurve animationCurve;
    
    public TileData data;
    public GameObject tileNormal;
    public GameObject tileBlocked;
    public TileType Type => data.tileType;

    private Tweener _bounce;
    
    [SerializeField] private GameObject _occupant;

    public GameObject Occupant
    {
        get => _occupant;
        set
        {
            _occupant = value;
            IsBlocked = _occupant;
        }
    }

    private bool _isBlocked;
    public bool IsBlocked
    {
        get => _isBlocked;
        set
        {
            _isBlocked = value;
            
            #if UNITY_EDITOR
            if (!AStar.instance.isDebugMode) return;
            tileNormal.gameObject.SetActive(!value);
            tileBlocked.gameObject.SetActive(value);
            #endif
        }
    }

    public Vector3 WorldPosition => transform.position;
    
    public Vector3 GridPosition { get; private set; }
    
    public Dictionary<Vector3, Tile> Neighbors { get; set; }

    private void OnEnable()
    {
        GridPosition = transform.localPosition;
    }

    private void Start()
    {
        tileNormal.gameObject.SetActive(true);
        tileBlocked.gameObject.SetActive(false);

        _bounce = transform
            /*.DOJump(transform.position, -0.2f, 1, 1f)*/
            .DOPunchPosition(new Vector3(0, -0.2f, 0), 0.5f, 2, 2)
            .Pause()
            .SetAutoKill(false)
            .OnComplete(() => transform.localPosition = GridPosition);

        BlockTileUnder();
    }

    private void BlockTileUnder()
    {
        Ray ray = new Ray(transform.position - new Vector3(0, 0.5f, 0), Vector3.down);
        RaycastHit hitInfo;
     
        if (!Physics.Raycast(ray, out hitInfo)) return;
        if (!hitInfo.collider.transform.GetComponentInParent<Tile>()) return;
        
        hitInfo.collider.transform.GetComponentInParent<Tile>().IsBlocked = true;
    }

    public void Bounce() => _bounce.Restart();
}

[Flags]
public enum TileType
{
    Normal = 1,
    Hideout = 2,
    None = 10
}
