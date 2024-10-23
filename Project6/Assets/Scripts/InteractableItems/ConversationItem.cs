using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationItem : InteractableItem
{
    [SerializeField] private ConversationManager conversationManager;
    [SerializeField] private SceneTransition sceneTransition;
    
    public override void AfterPressInteract()
    {
        StartCoroutine(InterationPerformance());
    }

    IEnumerator InterationPerformance()
    {
        yield return StartCoroutine(conversationManager.showAllConversation());
        yield return StartCoroutine(WaitSeconds(3));
        yield return StartCoroutine(sceneTransition.FadeOut());
    }

    IEnumerator WaitSeconds(float seconds)
    {
        // 等待3秒
        yield return new WaitForSeconds(seconds);
        
    }

}
