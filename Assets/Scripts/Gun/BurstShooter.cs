using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShooter : MonoBehaviour
{
    [Header("Shots")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public float projectileLifetime = 4f;

    [Header("Burst Shooting")]
    public int shotsPerBurst = 3;                // Quantos projéteis por rajada
    public float timeBetweenShots = 0.2f;        // Intervalo entre os tiros

    [Header("Recharge")]
    public float reloadTime = 1.5f;              // Tempo até poder atirar de novo
    private bool isReloading = false;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading)
        {
            StartCoroutine(FireBurst());
        }
    }

    IEnumerator FireBurst()
    {
        isReloading = true;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            FireProjectile();
            yield return new WaitForSeconds(timeBetweenShots);
        }

        // Aguarda recarga após disparar todos
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    void FireProjectile()
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
