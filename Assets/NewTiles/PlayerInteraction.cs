/*
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewTiles
{
    public class PlayerInteraction : MonoBehaviour
    {
        public SoundItem soundItem;

        public void OnSpeak(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (Player.State != PlayerState.Idle) return;
            if (soundItem != null) return;
            
            Player.State = PlayerState.Speaking;
            TileGrid.instance.onPlayerSoundStarted?.Invoke(soundItem);
            soundItem.audioEvent.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnFinishedSpeaking);
        }

        private void OnFinishedSpeaking(object inCookie, AkCallbackType inType, AkCallbackInfo inInfo)
        {
            TileGrid.instance.onPlayerSoundEnded?.Invoke(soundItem);
        }

        public void OnListen(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            Player.State = PlayerState.Listening;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }
    }
}
*/

