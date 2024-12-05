using System.Collections;
using System.Collections.Generic;
using LostFrame;
using UnityEngine;

public class Key : InteractableItem
{
    public KeyLocks keyLocks;
    
    public override void AfterPressInteract()
    {
        keyLocks.getKey = true;
        gameObject.SetActive(false);
        
    }
    
}