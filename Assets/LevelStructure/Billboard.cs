using System;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Start()
    {
        _mainCameraTransform = Camera.main?.transform;
        transform.rotation = Quaternion.LookRotation(-_mainCameraTransform.forward);
    }

    /*private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(-_mainCameraTransform.forward);
    }*/
}
