using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource calmMusic;
    [SerializeField] private AudioSource battleMusic;
    [SerializeField] private AudioSource deathMusic;

    [SerializeField, Range(0, 1)] private float calmMusicVolume = 1;
    [SerializeField, Range(0, 1)] private float battleMusicVolume = 1;
    [SerializeField, Range(0, 1)] private float deathMusicVolume = 1;

    [SerializeField, Range(1, 100)] private float battleMusicRange;

    [SerializeField, Range(0.05f, 4)] private float fadeSpeed;

    private PlayerInput playerInput;
    private Station[] stations;

    private bool isDead;

    private MusicState musicState;
    
    public enum MusicState
    {
        None,
        Calm,
        Battle,
        Death
    }


    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        stations = FindObjectsOfType<Station>();
        EventManager.instance.OnStationDestroyed += PlayDeathMusic;
        PlayCalmMusic();

    }
    
    private void Update()
    {
        bool battleMusicPlayed = false;
        
        for (int i = 0; i < stations.Length; i++)
        {
            if (stations[i].isBeingAttacked && playerInput.PlayerMovement != null)
            {
                if (Vector3.Distance(stations[i].transform.position, playerInput.PlayerMovement.transform.position) < battleMusicRange)
                {
                    PlayBattleMusic();
                    battleMusicPlayed = true;
                }
            }
        }
        
        if (!battleMusicPlayed)
        {
            PlayCalmMusic();
        }
    }

    private void PlayDeathMusic(int id)
    {
        if (!isDead)
        {
            Debug.Log("death");
            StartCoroutine(FadeToMusic(MusicState.Death));
            isDead = true;
        }
    }
    
    private void PlayCalmMusic()
    {
        if (!isDead && musicState != MusicState.Calm)
        {
            musicState = MusicState.Calm;
            StartCoroutine(FadeToMusic(MusicState.Calm));
        }
    }
    
    private void PlayBattleMusic()
    {
        if (musicState != MusicState.Battle && !isDead)
        {
            musicState = MusicState.Battle;
            StartCoroutine(FadeToMusic(MusicState.Battle));
        }
    }
    
    private IEnumerator FadeToMusic(MusicState musicToFadeIn)
    {
        Debug.Log("1 + " + musicToFadeIn);
        float fade = 0;

        while (fade < 1)
        {
            switch (musicToFadeIn)
            {
                case MusicState.None:
                    calmMusic.volume = Mathf.Lerp(calmMusic.volume, 0, fade);
                    battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0, fade);
                    deathMusic.volume = Mathf.Lerp(deathMusic.volume, 0, fade);
                    break;
                case MusicState.Calm:
                    calmMusic.volume = Mathf.Lerp(calmMusic.volume, calmMusicVolume, fade);
                    battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0, fade);
                    deathMusic.volume = Mathf.Lerp(deathMusic.volume, 0, fade);
                    break;
                
                case MusicState.Battle:
                    battleMusic.volume = Mathf.Lerp(battleMusic.volume, battleMusicVolume, fade);
                    calmMusic.volume = Mathf.Lerp(calmMusic.volume, 0, fade);
                    deathMusic.volume = Mathf.Lerp(deathMusic.volume, 0, fade);
                    break;
                
                case MusicState.Death:
                    deathMusic.volume = Mathf.Lerp(0, deathMusicVolume, fade);
                    calmMusic.volume = Mathf.Lerp(calmMusic.volume, 0, fade);
                    battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0, fade);
                    break;
            }

            fade += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

}
