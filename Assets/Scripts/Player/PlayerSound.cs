using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private SoundToPlay thrustSound;
    [SerializeField] private SoundToPlay boostSound;
    [SerializeField] private SoundToPlay fluxSound;
    
    private string thrustLoopInstanceName = "535445fr";
    private string boostLoopInstanceName = "sdddfweg";
    private string fluxInstanceName = "sdd5765df4we7g";

    private bool playingBoostSound;
    private bool playingThrustSound;

    public void StartThrust()
    {
        if (thrustSound)
        {
            AudioController.instance.PlayLoopingSoundAtPlayer(thrustSound.soundName, thrustLoopInstanceName,
                thrustSound.volume, thrustSound.pitch);
        }
    }

    public void StartBoost()
    {
        if (boostSound)
        {
            AudioController.instance.PlayLoopingSoundAtPlayer(boostSound.soundName, boostLoopInstanceName,
                boostSound.volume, boostSound.pitch);
        }
    }
    
    public void EndBoost()
    {
        AudioController.instance.StopSound(boostLoopInstanceName);
    }
    
    public void EndThrust()
    { 
        AudioController.instance.StopSound(thrustLoopInstanceName);
    }

    public void EnterFlux()
    {
        AudioController.instance.PlayLoopingSoundAtPlayer(fluxSound.soundName, fluxInstanceName, fluxSound.volume,
            fluxSound.pitch);
    }

    public void ExitFlux()
    {
        AudioController.instance.StopSound(fluxInstanceName);
    }
}
