using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

	private Transform camTransform;
	private Camera cam;

    private Dictionary<string, Coroutine> screenShakeDictionary = new Dictionary<string, Coroutine>();

	private void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        cam = GetComponentInChildren<Camera>();
		camTransform = cam.transform;
    }

	public void ShakeCam(float shakeDuration, float shakeAmount)
	{
		StartCoroutine(CameraShake(shakeDuration, shakeAmount));
	}

    public void ShakeCam(ScreenShakeData screenShakeData)
    {
        ShakeCam(screenShakeData.duration, screenShakeData.amount);
    }

    public void StartShake(string key, float shakeAmount)
    {
        key = key.ToLower();
        if (screenShakeDictionary.ContainsKey(key))
        {
            // This shake has already been started.
            return;
        }

        Coroutine newShake = StartCoroutine(CameraShakeInfinite(shakeAmount));
        screenShakeDictionary.Add(key, newShake);
    }

    public void StartShake(ScreenShakeData screenShakeData)
    {
        StartShake(screenShakeData.key, screenShakeData.amount);
    }

    public void StopShake(string key)
    {
        key = key.ToLower();

        Coroutine shake = null;
        if (screenShakeDictionary.TryGetValue(key, out shake))
        {
            StopCoroutine(shake);
            ResetPosition();
            screenShakeDictionary.Remove(key);
        }
    }

    private IEnumerator CameraShake(float duration, float shakeAmount)
	{
		float timer = duration;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
            ShakeRandomDir(shakeAmount);
            yield return new WaitForEndOfFrame();
		}

        ResetPosition();
    }

    private IEnumerator CameraShakeInfinite(float shakeAmount)
    {
        while (true)
        {
            ShakeRandomDir(shakeAmount);
            yield return new WaitForEndOfFrame();
        }
    }

    private void ShakeRandomDir(float shakeAmount)
    {
        Vector3 shakePos = Vector3.zero + Random.insideUnitSphere * shakeAmount;
        shakePos.z = camTransform.localPosition.z;
        camTransform.localPosition = shakePos;
    }

    private void ResetPosition()
    {
        Vector3 resetPos = Vector3.zero;
        resetPos.z = camTransform.localPosition.z;
        camTransform.localPosition = resetPos;
    }
}