using UnityEngine;

[CreateAssetMenu(fileName = "SoundItem", menuName = "Scriptable Objects/SoundItem")]
public class SoundItem : ScriptableObject
{
    public AK.Wwise.Event audioEvent;

    public AK.Wwise.Event playerEvent;

    public AK.Wwise.Event pipeEvent;
    //public AudioClip clip;
    public float reactionDelay;
}
