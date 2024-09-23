using System;
using System.Collections;
using Tools;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource playerAudioSource;
    private bool isMoving=false;
    
    private void Update()
    {
        // play character movement audio
        isMoving = playerAudioSource.gameObject.GetComponent<CharacterBasicPhysicalMovement>().isMoving;
        if (isMoving)
        {
            if (!playerAudioSource.isPlaying) playerAudioSource.Play(0);
        }else
            playerAudioSource.Pause();
    }

    public IEnumerator WeakSound(AudioSource audioSource,float targetVolume,float startVolume, float speedPerPointSecond)
    {
        audioSource.volume = startVolume;
        audioSource.Play(0);
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += speedPerPointSecond;
            yield return new WaitForSeconds(0.1f);
        }
        
    }   
}