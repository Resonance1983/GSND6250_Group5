using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Tools;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [DisallowNull]
    [SerializeField] private AudioSource playerAudioSource;
    private bool isMoving=false;
    
    private void Update()
    {
        // player character movement audio
        if(playerAudioSource.gameObject.TryGetComponent<CharacterPhysicalMovement>(out CharacterPhysicalMovement cpm))
            isMoving = cpm.isMoving;
        if (isMoving)
        {
            if (!playerAudioSource.isPlaying) playerAudioSource.Play(0);
        }else
            playerAudioSource.Pause();
    }

    public IEnumerator AwakeSound(AudioSource audioSource,float targetVolume,float startVolume, float speedPerPointSecond)
    {
        audioSource.volume = startVolume;
        audioSource.Play(0);
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += speedPerPointSecond;
            yield return new WaitForSeconds(0.1f);
        }
        
    }   
    
    public IEnumerator ExtinguishSound(AudioSource audioSource,float targetVolume,float startVolume, float speedPerPointSecond)
    {
        audioSource.volume = startVolume;
        audioSource.Play(0);
        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= speedPerPointSecond;
            yield return new WaitForSeconds(0.1f);
        }
        
    }   
    
}