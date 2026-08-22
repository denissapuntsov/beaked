using UnityEngine;
using UnityEngine.Events;

    public class Interactable : MonoBehaviour
    {
        public UnityEvent onInteract;
        
        public void Interact()
        {
            //PopupManager.instance.HidePopup(PlayerData.Instance.gameObject);
            onInteract.Invoke();
        }
    }

