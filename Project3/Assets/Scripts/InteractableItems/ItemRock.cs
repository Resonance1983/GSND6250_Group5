using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRock : InteractableItem
{
    public CharacterRayCast playerRayCast;
    public override void AfterPressInteract()
    {
        if (playerRayCast != null)
        {
            playerRayCast.isEnabled = true;
            Debug.Log(playerRayCast.isEnabled);
        }
        
        interactionTips.SetActive(false);
        gameObject.SetActive(false);
    }
    
}
