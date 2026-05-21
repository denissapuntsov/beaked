using UnityEngine;

[CreateAssetMenu(fileName = "SoundItem", menuName = "Scriptable Objects/SoundItem")]
public class SoundItem : ScriptableObject
{
    public AudioClip clip;
    [Range(1, 10)] public int range;
}
