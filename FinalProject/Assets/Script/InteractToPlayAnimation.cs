using System.Collections;
using System.Collections.Generic;
using LostFrame;
using UnityEngine;

public class InteractToPlayAnimation : InteractableItem
{
    
    public override void AfterPressInteract()
    {
        GetComponent<Animation>().Play();

    }
    
}