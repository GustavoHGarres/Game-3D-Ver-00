using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedShotGun : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 20f;
    public int maxAmmo = 5;
    public float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);

        currentAmmo--;
        Debug.Log("Tiro! Munição restante: " + currentAmmo);

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recarregando...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Recarregado.");
    }

    // Atualizando para a UI
    public int GetCurrentAmmo()
    {
       return currentAmmo;
    }

    public bool IsReloading()
    {
       return isReloading;
    }
    // Atualizando para a UI
}
