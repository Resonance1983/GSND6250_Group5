using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public float waitDuration = 2.0f;
    public AnimationCurve fadeCurve;
    public Text conversationText;
    public List<String> ConversationContent = new List<String>();
    
    public IEnumerator showSingleText(int num)
    {
        float elapsedTime = 0f;
        Color textColor = conversationText.color;
        textColor.a = 0;
        conversationText.text = ConversationContent[num];
        
        // 渐入
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
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
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
            textColor.a = Mathf.Clamp01(1.0f - curveValue);
            conversationText.color = textColor;
            yield return null;
        }
        
    }
    public IEnumerator showConversation(int startNum, int endNum)
    {
        for (int i = startNum; i <= endNum; i++)
        {
            yield return StartCoroutine(showSingleText(i));
        }
    }

    public IEnumerator showAllConversation()
    {
        yield return StartCoroutine(showConversation(0, ConversationContent.Count));
    }


}
