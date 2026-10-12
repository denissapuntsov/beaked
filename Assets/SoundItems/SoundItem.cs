using UnityEngine;

[CreateAssetMenu(fileName = "SoundItem", menuName = "Scriptable Objects/SoundItem")]
public class SoundItem : ScriptableObject
{
    public AK.Wwise.Event audioEvent;
    //public AudioClip clip;
    public float reactionDelay;
}
