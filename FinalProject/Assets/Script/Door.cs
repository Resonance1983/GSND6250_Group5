using System.Collections;
using System.Collections.Generic;
using LostFrame;
using UnityEngine;

public class Door : InteractableItem
{
    public KeyLocks keyLocks;
    
    public override void AfterPressInteract()
    {
        if (keyLocks.getKey)
        {
            keyLocks.openDoor = true;
            gameObject.SetActive(false);
            
        }
        
    }
    
}