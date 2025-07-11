using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeGun : MonoBehaviour

{
    [Header("Tiro")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float minChargeTime = 1.5f;
    public float maxChargeTime = 3f;
    public float baseProjectileSpeed = 15f;
    public float projectileLifetime = 5f;

    [Header("Visual")]
    public GameObject chargeIndicator;

    [Header("Som")]
    public AudioClip chargeSound;
    public AudioClip shootSound;
    public AudioSource audioSource;

    private float chargeTimer = 0f;
    private bool isCharging = false;

    void Update()
    {
        // Início do carregamento
        if (Input.GetButtonDown("Fire1"))
        {
            isCharging = true;
            chargeTimer = 0f;

            if (chargeIndicator != null)
            {
                chargeIndicator.SetActive(true);
                chargeIndicator.transform.localScale = Vector3.zero;
            }

            if (audioSource != null && chargeSound != null)
                audioSource.PlayOneShot(chargeSound);
        }

        // Durante carregamento
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            if (chargeIndicator != null)
            {
                float scale = Mathf.Clamp01(chargeTimer / maxChargeTime);
                chargeIndicator.transform.localScale = Vector3.one * scale;
            }
        }

        // Solta botão
        if (Input.GetButtonUp("Fire1"))
        {
            if (chargeTimer >= minChargeTime)
            {
                FireProjectile();
                if (audioSource != null && shootSound != null)
                    audioSource.PlayOneShot(shootSound);
            }

            isCharging = false;
            chargeTimer = 0f;

            if (chargeIndicator != null)
                chargeIndicator.SetActive(false);
        }
    }

    void FireProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        float chargeRatio = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float finalSpeed = baseProjectileSpeed * Mathf.Lerp(1f, 2f, chargeRatio);

        if (rb != null)
            rb.velocity = firePoint.forward * finalSpeed;

        Destroy(proj, projectileLifetime);
    }
}