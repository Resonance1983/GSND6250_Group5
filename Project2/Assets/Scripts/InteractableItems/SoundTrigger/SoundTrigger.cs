using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundTrigger : InteractableItem
{
    private AudioSource audioSource;
    [SerializeField] private bool exitToPauseSound = true;
    
    public override void InteractableItemOnTriggerEnter(Collider other)
    {
        print("trigger enter");
        if (other.tag.Equals("Player"))
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            print("audio play");
            audioSource.Play(0);
        }
        
    }

    public override void InteractableItemOnTriggerExit(Collider other)
    {
        if (exitToPauseSound && other.tag.Equals("Player"))
        {
            audioSource.Pause();
        }
    }
    
}
