using System;
using UnityEngine;

namespace NewTiles
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        public static PlayerState State { get; set; } = PlayerState.Idle;

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);
        }
    }  

    public enum PlayerState
    {
        Idle,
        Moving,
        Speaking,
        Listening
    }
}
