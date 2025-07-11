using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TiroCarregado : MonoBehaviour

{
    public GameObject projectilePrefab;   // O prefab do projétil
    public Transform firePoint;           // De onde o projétil será disparado
    public float minChargeTime = 1.5f;    // Tempo mínimo para considerar carregado
    public float projectileSpeed = 20f;   // Velocidade do projétil
    public float projectileLifetime = 5f; // Tempo até o projétil se destruir

    private float chargeTimer = 0f;
    private bool isCharging = false;

    public UIChargeBar uiChargeBar; // Referência ao script da UI

    void Update()
    {

#region Tiros com o mouse

        // Início do carregamento
        if (Input.GetButtonDown("Fire1"))
        {
            isCharging = true;
            chargeTimer = 0f;
            uiChargeBar.ShowBar(true); // Mostrar barra
        }

        // Carregando
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            float normalizedCharge = Mathf.Clamp01(chargeTimer / minChargeTime); // UI
            uiChargeBar.UpdateChargeBar(normalizedCharge); // UI
        }

        // Ao soltar o botão
        if (Input.GetButtonUp("Fire1"))
        {
            if (chargeTimer >= minChargeTime)
            {
                ShootProjectile();
            }
            isCharging = false;
             uiChargeBar.ShowBar(false); // Esconder barra
        }

#endregion 

#region Tiros com o teclado

// Início do carregamento
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTimer = 0f;
            uiChargeBar.ShowBar(true); // Mostrar barra
        }

        // Carregando
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            float normalizedCharge = Mathf.Clamp01(chargeTimer / minChargeTime); // UI
            uiChargeBar.UpdateChargeBar(normalizedCharge); // UI
        }

        // Ao soltar o botão
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (chargeTimer >= minChargeTime)
            {
                ShootProjectile();
            }
            isCharging = false;
             uiChargeBar.ShowBar(false); // Esconder barra
        }

#endregion

    }

    void ShootProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        Destroy(proj, projectileLifetime);
    }
}