using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnEnable : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}