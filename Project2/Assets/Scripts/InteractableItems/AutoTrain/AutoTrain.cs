using UnityEngine;

public class AutoTrain : InteractableItem
{
    public Animator doorAnimator;
    public Animator trainAnimator;
    
    private bool once = false;

    public override void InteractableItemOnTriggerExit()
    {
        doorAnimator.SetBool("IsOpenning", false);
        trainAnimator.SetBool("IsTrainning",true);
        
    }

    public override void InteractableItemOnTriggerEnter()
    {
        if (!once)
        {
            doorAnimator.SetBool("IsOpenning", true);
            once = !once;
        }
        
    }

}
