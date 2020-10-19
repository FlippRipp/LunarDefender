using UnityEngine;

public class ObjectEventCaller : MonoBehaviour
{
    private const float SCREEN_SHAKE_SMALL = 0.1f;
    private const float SCREEN_SHAKE_MEDIUM = 0.2f;
    private const float SCREEN_SHAKE_BIG = 0.4f;

    public GameObject[] objectsToSpawn;

    public void QuickScreenShake(float amount)
    {
        ScreenShake.instance.ShakeCam(0.1f, amount);
    }

    public void SmallScreenShake(float duration)
    {
        ScreenShake.instance.ShakeCam(duration, SCREEN_SHAKE_SMALL);
    }

    public void MediumScreenShake(float duration)
    {
        ScreenShake.instance.ShakeCam(duration, SCREEN_SHAKE_MEDIUM);
    }

    public void BigScreenShake(float duration)
    {
        ScreenShake.instance.ShakeCam(duration, SCREEN_SHAKE_BIG);
    }

    public void ShakeOneShot(ScreenShakeData screenShakeData)
    {
        ScreenShake.instance.ShakeCam(screenShakeData);
    }

    public void StartShake(ScreenShakeData screenShakeData)
    {
        ScreenShake.instance.StartShake(screenShakeData);
    }

    public void PlaySoundAtPlayer(SoundToPlay sound)
    {
        AudioController.instance.PlaySoundAtPlayer(sound.soundName, sound.volume, sound.pitch);
    }
    
    public void PlaySoundAtPosition(SoundToPlay sound)
    {
        AudioController.instance.PlaySoundAtPosition(sound.soundName, transform.position, sound.volume, sound.pitch);
    }


    public void StopShake(ScreenShakeData screenShakeData)
    {
        ScreenShake.instance.StopShake(screenShakeData.key);
    }

    public void SpawnObject(int objectsToSpawnIndex)
    {
        Instantiate(objectsToSpawn[objectsToSpawnIndex], transform.position, Quaternion.identity);
    }
}
