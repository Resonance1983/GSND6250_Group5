using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public float fadeDuration = 10.0f;
    public AnimationCurve fadeCurve;
    public Text transitionText;
    private Image blackScreen;

    void Start()
    {
        CreateBlackScreen();
        StartCoroutine(FadeOut());
    }

    void CreateBlackScreen()
    {
        GameObject canvasGO = GameObject.Find("Canvas");

        GameObject imageGO = new GameObject("BlackScreen");
        imageGO.transform.SetParent(canvasGO.transform, false);
        blackScreen = imageGO.AddComponent<Image>();
        blackScreen.color = Color.black;
        
        // 将文本的优先级提高防止被黑幕遮挡
        transitionText.transform.SetSiblingIndex(blackScreen.transform.GetSiblingIndex() + 1);

        RectTransform rectTransform = imageGO.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        transitionText.transform.SetParent(canvasGO.transform, false);
        transitionText.color = new Color(transitionText.color.r, transitionText.color.g, transitionText.color.b, 0);
    }

    public IEnumerator FadeTextAndWait()
    {
        float elapsedTime = 0f;
        Color textColor = transitionText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
            textColor.a = Mathf.Clamp01(curveValue);
            transitionText.color = textColor;
            yield return null;
        }

        // 等待3秒
        yield return new WaitForSeconds(3.0f);

        // 跳转到下一个场景
        SceneManager.LoadScene("NextSceneName"); // 替换为你的场景名称
    }
    
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = blackScreen.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
            color.a = Mathf.Clamp01(curveValue);
            blackScreen.color = color;

            // fade out 时显示文字，不需要可以删掉
            if (color.a >= 0.5f)
            {
                StartCoroutine(FadeTextAndWait());
            }

            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = blackScreen.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
            color.a = Mathf.Clamp01(1.0f - curveValue);
            blackScreen.color = color;
            yield return null;
        }
    }
}
