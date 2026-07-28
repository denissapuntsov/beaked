using UnityEngine;

namespace NewTiles
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public static PlayerState State { get; set; } = PlayerState.Idle;
    }

    public enum PlayerState
    {
        Idle,
        Moving,
        Speaking,
        Listening
    }
}
