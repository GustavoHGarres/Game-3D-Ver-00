using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun_Particle : MonoBehaviour
{
    [Header("Tiro")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public float projectileLifetime = 4f;

    [Header("Disparo em Rajada")]
    public int shotsPerBurst = 3;
    public float timeBetweenShots = 0.2f;

    [Header("Recarga")]
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("Partículas e Efeitos")]
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioSource audioSource;

    [Header("UI (opcional)")]
    public Slider reloadSlider;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading)
        {
            StartCoroutine(FireBurst());
        }

        
        if (Input.GetKeyUp(KeyCode.X) && !isReloading)
        {
            StartCoroutine(FireBurst());
        }

        // Atualiza barra de recarga se estiver sendo usada
        if (isReloading && reloadSlider != null)
        {
            reloadSlider.value += Time.deltaTime / reloadTime;
        }
    }

    IEnumerator FireBurst()
    {
        isReloading = true;

        if (reloadSlider != null)
        {
            reloadSlider.value = 0f;
        }

        for (int i = 0; i < shotsPerBurst; i++)
        {
            FireProjectile();
            yield return new WaitForSeconds(timeBetweenShots);
        }

        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);
        isReloading = false;

        if (reloadSlider != null)
        {
            reloadSlider.value = 1f;
        }
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

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }
}