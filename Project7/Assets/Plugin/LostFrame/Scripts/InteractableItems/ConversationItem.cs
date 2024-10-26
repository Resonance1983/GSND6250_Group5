using System.Collections;
using UnityEngine;

namespace LostFrame
{
    public class ConversationItem : InteractableItem
    {
        [SerializeField] private ConversationManager conversationManager;
        [SerializeField] private SceneTransition sceneTransition;

        public override void AfterPressInteract()
        {
            StartCoroutine(InterationPerformance());
        }

        private IEnumerator InterationPerformance()
        {
            yield return StartCoroutine(conversationManager.showAllConversation());
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