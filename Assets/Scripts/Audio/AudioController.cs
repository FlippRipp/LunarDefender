using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>(); //<name, audio clip>
    private Dictionary<string, GameObject> runningSounds = new Dictionary<string, GameObject>(); //<name, audio clip>
    
    [SerializeField] private float defaultVolume = 1f;
    [SerializeField] private GameObject audioPrefab;
    [SerializeField]private KeyValue[] soundInspectorList;
    
    private Transform playerTf;
    
    [Serializable]
    public struct KeyValue
    {
        public string audioName;
        public AudioClip audioFile;

        public KeyValue(string k, AudioClip v)
        {
            audioName = k;
            audioFile = v;
        }
    }

    public static AudioController instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        for (int i = 0; i < soundInspectorList.Length; i++)
        {
            sounds.Add(soundInspectorList[i].audioName, soundInspectorList[i].audioFile);
        }
    }

    public void PlaySoundAtPlayer(string soundName)
    {
        if (!playerTf)
        {
            playerTf = GetPlayer();
        }

        PlaySound(soundName, playerTf.position, defaultVolume);
    }
    
    public void PlaySoundAtPlayer(string soundName, float volume, float pitch)
    {
        if (!playerTf)
        {
            playerTf = GetPlayer();
        }

        PlaySound(soundName, playerTf, volume, "", false , pitch);
    }


    private void PlaySound(string soundname, Vector3 position, float volume, string instanceName = "", bool loop = false, float pitch = 1)
    {
        GameObject soundObject = Instantiate(audioPrefab, position, Quaternion.identity);
        AudioSource aS = soundObject.GetComponent<AudioSource>();
        aS.volume = volume;
        aS.pitch = pitch;
        aS.clip = sounds[soundname];
        aS.Play();

        if (!string.IsNullOrEmpty(instanceName) && !runningSounds.ContainsKey(instanceName))
        {
            runningSounds.Add(instanceName, soundObject);
        }
        
        if (loop)
        {
            aS.loop = true;
        }
        else
        {
            Destroy(soundObject, sounds[soundname].length);
        }
    }
    
    private void PlaySound(string soundname, Transform tf, float volume, string instanceName = "", bool loop = false, float pitch = 1)
    {
        GameObject soundObject = Instantiate(audioPrefab, tf.position, Quaternion.identity);
        soundObject.transform.parent = tf;
        AudioSource aS = soundObject.GetComponent<AudioSource>();
        aS.volume = volume;
        aS.pitch = pitch;
        aS.clip = sounds[soundname];
        aS.Play();

        if (!string.IsNullOrEmpty(instanceName))
        {
            runningSounds.Add(instanceName, soundObject);
        }
        
        if (loop)
        {
            aS.loop = true;
        }
        else
        {
            Destroy(soundObject, sounds[soundname].length);
        }
    }
    
    public void PlaySoundAtPlayer(string soundName, float volume, string instanceName = "")
    {
        if (!playerTf)
        {
            playerTf = GetPlayer();
        }
        
        PlaySound(soundName, playerTf.position, volume, instanceName);
    }

    public float GetSoundLenght(string soundName)
    {
        return sounds[soundName].length;
    }

    public void PlayLoopingSoundAtPlayer(string soundName, string instanceName, float pitch = 1)
    {
        if (!playerTf)
        {
            playerTf = GetPlayer();
        }

        PlaySound(soundName, playerTf, defaultVolume, instanceName, true, pitch);
    }
    
    public void PlayLoopingSoundAtPlayer(string soundName, string instanceName, float volume, float pitch)
    {
        if (!playerTf)
        {
            playerTf = GetPlayer();
        }

        PlaySound(soundName, playerTf, volume, instanceName, true, pitch);
    }

    
    public void StopSound(string instanceName)
    {
        if(!runningSounds.ContainsKey(instanceName)) return;
        Destroy(runningSounds[instanceName]);
        runningSounds.Remove(instanceName);
    }

    public void PlaySoundAtPosition(string soundName ,Vector3 position)
    {
        PlaySound(soundName, position, defaultVolume);
    }

    public void PlaySoundAtPosition(string soundName ,Vector3 position, float volume)
    {
        PlaySound(soundName, position, volume);
    }
    public void PlaySoundAtPosition(string soundName ,Vector3 position, float volume, float pitch)
    {
        PlaySound(soundName, position, volume, "", false, pitch);
    }

    private Transform GetPlayer()
    {
        return PlayerInput.instance.PlayerMovement.transform;
    }

}
