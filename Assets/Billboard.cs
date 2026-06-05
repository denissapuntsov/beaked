using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Start()
    {
        _mainCameraTransform = Camera.main?.transform;
    }

    private void Update()
    {
        transform.LookAt(transform.position + _mainCameraTransform.forward);
    }
}
