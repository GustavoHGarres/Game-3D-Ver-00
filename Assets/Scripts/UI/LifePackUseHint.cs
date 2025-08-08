using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Itens;

public class LifePackUseHint : MonoBehaviour
{
    [Header("UI (filho visual)")]
    [SerializeField] private CanvasGroup visualGroup;   // CanvasGroup do Text (filho)
    [SerializeField] private TextMeshProUGUI hintText;  // opcional
    [SerializeField] private KeyCode useKey = KeyCode.L;

    private SOInt lifeSoInt;
    private int lastValue = int.MinValue;

    private void Start()
    {
        // Pega o SOInt do Life Pack
        lifeSoInt = ItemManager.Instance.GetItemByType(ItemType.LIFE_PACK).soInt;

        if (hintText != null)
            hintText.text = $"Pressione [{useKey}] para usar o LifePack";

        Apply(); // estado inicial
    }

    private void Update()
    {
        if (lifeSoInt == null) return;

        if (lifeSoInt.value != lastValue)
        {
            lastValue = lifeSoInt.value;
            Apply();
        }
    }

    private void Apply()
    {
        bool show = lifeSoInt != null && lifeSoInt.value > 0;

        if (visualGroup != null)
        {
            visualGroup.alpha = show ? 1f : 0f;
            visualGroup.interactable = false;
            visualGroup.blocksRaycasts = false;
        }
    }
}