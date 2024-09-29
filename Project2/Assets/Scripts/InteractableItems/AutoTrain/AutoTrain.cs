using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTrain : InteractableItem
{
    public Animator doorAnimator;
    public Animator trainAnimator;
    public AudioSource audioSource;
    
    private bool once = false;

    public override void InteractableItemOnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            doorAnimator.SetBool("IsOpenning", false);
            trainAnimator.SetBool("IsTrainning",true);
        }
        
    }

    public override void InteractableItemOnTriggerEnter(Collider other)
    {
        if (!once && other.tag.Equals("Player"))
        {
            doorAnimator.SetBool("IsOpenning", true);
            other.GetComponent<CharacterMovement_TrackMomentum>().connectRb =
                gameObject.GetComponent<Rigidbody>();
            other.GetComponent<CharacterPhysicalMovement>().setIsCancelMaxSpeed(true);
            once = !once;
        }
        
    }

    public void cancelConnectRbForAnimationEvent() {
        GameObject player = GameObject.Find("Player");
        audioSource.Stop();
        player.GetComponent<CharacterMovement_TrackMomentum>().connectRb = null;
        player.GetComponent<CharacterPhysicalMovement>().setIsCancelMaxSpeed(true);
    }

    public void awakeTrainSound()
    {
        StartCoroutine(GameObject.Find("SoundManager").GetComponent<SoundManager>().AwakeSound(audioSource, 0.5f, 0, 0.02f));
    }

    public void nextScene()
    {
        SceneManager.LoadScene("FirstDraftscene");
    }


}
