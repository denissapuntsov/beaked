using System;
using UnityEngine;

namespace NewTiles
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        
        /*private Listener[] _listeners;*/

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(SoundItem sound, Cell cell, AkCallbackManager.EventCallback onFinishedSpeaking)
        {
            sound.audioEvent.Post(cell.occupyingElement.gameObject, (uint)AkCallbackType.AK_EndOfEvent, onFinishedSpeaking);
            ScanForValidListeners(sound);
        }
        
        private void ScanForValidListeners(SoundItem sound)
        {
            /*_listeners = new Listener[] { };*/
        }

        /*private void PrimeListeners()
        {
            foreach (var listener in _listeners)
            {
                
            }
        }

        private void TriggerListeners(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
        {
            foreach (var listener in _listeners)
            {
                
            }
        }*/
    }
    
}
