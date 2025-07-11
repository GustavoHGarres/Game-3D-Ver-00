using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGunShootLimit : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    public LimitedShotGun gun;

    void Update()
    {
        if (gun == null) return;

        ammoText.text = "Munição: " + gun.GetCurrentAmmo() + " / " + gun.maxAmmo;
        reloadText.text = gun.IsReloading() ? "Recarregando..." : "";
    }
}
