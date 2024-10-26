using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostFrame
{
    public class ConversationItem : InteractableItem
    {
        [SerializeField] private ConversationManager conversationManager;
        [SerializeField] private SceneTransition sceneTransition;
        
        public List<string> ConversationContent = new List<string>();

        public override void AfterPressInteract()
        {
            StartCoroutine(InterationPerformance());
        }

        private IEnumerator InterationPerformance()
        {
            yield return StartCoroutine(conversationManager.showConversationWithStringList(ConversationContent));
            yield return StartCoroutine(WaitSeconds(3));
            yield return StartCoroutine(sceneTransition.FadeOut());
        }

        private IEnumerator WaitSeconds(float seconds)
        {
            // 等待3秒
            yield return new WaitForSeconds(seconds);
        }
    }
}