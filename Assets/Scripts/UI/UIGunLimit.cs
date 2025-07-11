using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGunLimit : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI statusText;
    public Slider reloadBar;

    public void UpdateAmmo(int current, int max)
    {
        if (ammoText != null)
            ammoText.text = $"Tiros: {current}/{max}";
    }

    public void SetStatus(string status)
    {
        if (statusText != null)
            statusText.text = status;
    }

    //Slider
    public void ShowReloadBar(bool show)
    {
        if (reloadBar != null)
            reloadBar.gameObject.SetActive(show);
    }

    public void UpdateReloadBar(float value)
    {
        if (reloadBar != null)
            reloadBar.value = value;
    }
    //Slider
}