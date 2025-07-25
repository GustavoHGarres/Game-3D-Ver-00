using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIFillUpdater : MonoBehaviour
{
    public List<Image> uiImages; // Pode arrastar quantas quiser no Inspector

    [Header("Animation UI")]
    public float duration = .1f;
    public Ease ease = Ease.OutBack;

    private Tween _crrTween;

    private void OnValidate()
    {
        if (uiImages.Count == 0)
        {
            var image = GetComponent<Image>();
            if (image != null)
                uiImages.Add(image);
        }
    }

    public void UpdateValue(float value)
    {
        foreach (var img in uiImages)
        {
            img.fillAmount = value;
        }
    }

    public void UpdateValue(float max, float current)
    {
        float value = current / max;

        foreach (var img in uiImages)
        {
            if (_crrTween != null) _crrTween.Kill();
            _crrTween = img.DOFillAmount(value, duration).SetEase(ease);
        }
    }
}