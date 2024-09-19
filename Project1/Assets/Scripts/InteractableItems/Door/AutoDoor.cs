using UnityEngine;

public class AutoDoor : InteractableItem
{
    [SerializeField] private Animator animator;

    public override void InteractableItemOnTriggerExit()
    {
        animator.SetBool("IsOpenning", false);
    }

    public override void AfterPressInteract()
    {
        animator.SetBool("IsOpenning", true);
    }

}
