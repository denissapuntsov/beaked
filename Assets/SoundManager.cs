using System;
using UnityEngine;

namespace NewTiles
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

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
        }
    }
    
}
