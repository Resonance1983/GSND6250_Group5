using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationItem : InteractableItem
{
    [SerializeField] private ConversationManager cm;
    
    void AfterPressInteract()
    {
        StartCoroutine(cm.showAllConversation());
    }
}
