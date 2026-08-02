using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;

    public string Text
    {
        set => textField.text = value;
    }
    
    public void SetActive(bool active) => gameObject.SetActive(active);
}
