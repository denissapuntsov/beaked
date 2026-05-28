using System;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Player>())
        {
            return;
        }
        
        Debug.Log("collected");
        gameObject.SetActive(false);
    }
}
