using UnityEngine;

[CreateAssetMenu(fileName = "SoundToPlay", menuName = "Lunar Defender/SoundToPlay")]
public class SoundToPlay : ScriptableObject
{
    public string soundName = "";
    [Range(0, 1)]public float volume = 1;
    public float pitch = 1;
}
