using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LostFrame
{
    public class ConversationManager : MonoBehaviour
    {
        public float fadeDuration = 1.0f;
        public float waitDuration = 2.0f;
        public AnimationCurve fadeCurve;
        public Text conversationText;
        public List<string> ConversationContent = new();
        public String transitionText;

        private void Start()
        {
            // GameObject.Find("SceneTransition").transitionText = transitionText;
        }

        public IEnumerator showSingleText(int num)
        {
            var elapsedTime = 0f;
            var textColor = conversationText.color;
            textColor.a = 0;
            conversationText.text = ConversationContent[num];

            // 渐入
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
                textColor.a = Mathf.Clamp01(curveValue);
                conversationText.color = textColor;
                yield return null;
            }

            // 等待完
            elapsedTime = 0f;
            while (elapsedTime < waitDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 渐出
            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
                textColor.a = Mathf.Clamp01(1.0f - curveValue);
                conversationText.color = textColor;
                yield return null;
            }
        }

        public IEnumerator showConversation(int startNum, int endNum)
        {
            for (var i = startNum; i <= endNum; i++) yield return StartCoroutine(showSingleText(i));
        }

        public IEnumerator showAllConversation()
        {
            yield return StartCoroutine(showConversation(0, ConversationContent.Count - 1));
        }
    }
}