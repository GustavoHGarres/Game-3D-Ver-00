using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Scroll do mouse
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeapon++;
            if (selectedWeapon >= transform.childCount)
                selectedWeapon = 0;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeapon--;
            if (selectedWeapon < 0)
                selectedWeapon = transform.childCount - 1;
        }

        // Teclas numéricas
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            selectedWeapon = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            selectedWeapon = 3;    
        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
            selectedWeapon = 4;    
        if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6)
            selectedWeapon = 5;

        // Se mudou, atualiza a arma ativa
        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}