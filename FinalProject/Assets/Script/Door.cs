using System.Collections;
using System.Collections.Generic;
using LostFrame;
using TMPro;
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

    public override void InteractableItemOnTriggerEnter(Collider other)
    {
        if (!keyLocks.getKey)
        {
            interactionTips.GetComponent<TextMeshProUGUI>().text = "Door locked";
        }
    }

    public override void InteractableItemOnTriggerExit(Collider other)
    {
        interactionTips.GetComponent<TextMeshProUGUI>().text = "Press 'F'";
    }

}