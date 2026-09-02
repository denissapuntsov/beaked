using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Start()
    {
        _mainCameraTransform = Camera.main?.transform;
    }

    private void LateUpdate()
    {
        Vector3 direction = transform.position - _mainCameraTransform.position;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }
        
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
