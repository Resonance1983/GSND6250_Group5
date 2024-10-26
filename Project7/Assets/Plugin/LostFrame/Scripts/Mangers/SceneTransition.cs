using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LostFrame
{
    public class SceneTransition : MonoBehaviour
    {
        // User Setting
        public float fadeDuration = 8.0f;
        public AnimationCurve fadeCurve;
        public string nextSceneName;
        // public string nextWeatherName;
        
        [HideInInspector]
        public Text transitionText;
        private Image blackScreen;

        private void Start()
        {
            CreateBlackScreen();
            StartCoroutine(FadeIn());
        }

        private void CreateBlackScreen()
        {
            var canvasGO = GameObject.Find("Canvas");

            var imageGO = new GameObject("BlackScreen");
            imageGO.transform.SetParent(canvasGO.transform, false);
            blackScreen = imageGO.AddComponent<Image>();
            blackScreen.color = Color.black;

            // 将文本的优先级提高防止被黑幕遮挡
            transitionText.transform.SetSiblingIndex(blackScreen.transform.GetSiblingIndex() + 1);

            var rectTransform = imageGO.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;

            transitionText.transform.SetParent(canvasGO.transform, false);
            transitionText.color = new Color(transitionText.color.r, transitionText.color.g, transitionText.color.b, 0);
        }

        public IEnumerator FadeTextAndWait()
        {
            var elapsedTime = 0f;
            var textColor = transitionText.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
                textColor.a = Mathf.Clamp01(curveValue);
                transitionText.color = textColor;
                yield return null;
            }

            // 等待3秒
            yield return new WaitForSeconds(3.0f);

            // 跳转到下一个场景并更换天气(需要的话)
            SceneManager.LoadScene(nextSceneName);
            // Enviro.EnviroManager.instance.Weather.ChangeWeather(nextWeatherName);
        }

        public IEnumerator FadeOut()
        {
            var elapsedTime = 0f;
            var color = blackScreen.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
                color.a = Mathf.Clamp01(curveValue);
                blackScreen.color = color;

                // fade out 时显示文字，不需要可以删掉
                if (color.a >= 0.5f) StartCoroutine(FadeTextAndWait());

                yield return null;
            }
        }

        public IEnumerator FadeIn()
        {
            var elapsedTime = 0f;
            var color = blackScreen.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
                color.a = Mathf.Clamp01(1.0f - curveValue);
                blackScreen.color = color;
                yield return null;
            }
        }
    }
}