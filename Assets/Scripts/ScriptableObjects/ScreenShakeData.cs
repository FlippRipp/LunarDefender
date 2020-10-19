using UnityEngine;

[CreateAssetMenu(fileName = "ScreenShake_", menuName = "Lunar Defender/Screen Shake")]
public class ScreenShakeData : ScriptableObject
{
    [Tooltip("The screen shake key is used to identify currently active " +
        "shakes and stop them. Not needed for one-shot shakes.")]
    public string key;
    public float amount = 0.1f;
    public float duration = 0.1f;
}
