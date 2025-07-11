using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChargeBar : MonoBehaviour
{
    // Este script controla a barra de carregamento
    public Slider chargeSlider;

    public Image fillImage; // Referência à imagem de preenchimento

    public Color lowColor = Color.red;
    public Color midColor = Color.yellow;
    public Color highColor = Color.green;

    public Transform pulseTarget;  // Objeto que vai pulsar (normalmente o Fill)
    public float pulseScale = 1.05f;   // Quanto aumenta
    public float pulseSpeed = 3f;      // Velocidade do efeito

    private bool isFullyCharged = false;

    public void UpdateChargeBar(float value)
    {
        chargeSlider.value = value;

        // Interpola as cores dependendo do valor
        if (value < 0.5f)
        {
            fillImage.color = Color.Lerp(lowColor, midColor, value * 2f);
        }
        else
        {
            fillImage.color = Color.Lerp(midColor, highColor, (value - 0.5f) * 2f);
        }
        // Interpola as cores dependendo do valor

        // Ativar ou desativar pulsação
        isFullyCharged = (value >= 1f);
    }

    public void ShowBar(bool show)
    {
        chargeSlider.gameObject.SetActive(show);

        if (!show && pulseTarget != null)
        {
            pulseTarget.localScale = Vector3.one; // Reset ao esconder
        }
    }

    void Update()
    {
        // Animação de pulsar quando carregado
        if (isFullyCharged && pulseTarget != null)
        {
            float scale = 1f + Mathf.PingPong(Time.time * pulseSpeed, pulseScale - 1f);
            pulseTarget.localScale = new Vector3(scale, scale, 1f);
        }
        else if (pulseTarget != null)
        {
            // Volta ao normal quando não está carregado
            pulseTarget.localScale = Vector3.one;
        }
    }


}