using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationItem : InteractableItem
{
    [SerializeField] private ConversationManager conversationManager;
    
    public override void AfterPressInteract()
    {
        StartCoroutine(conversationManager.showAllConversation());
    }
}
