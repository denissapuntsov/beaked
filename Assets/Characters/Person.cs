using UnityEngine;

public class Person : MonoBehaviour
{
    public GameObject item;
    
    public void DropItem()
    {
        item.gameObject.SetActive(true);
    }
}
