using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWithEjectedShell : MonoBehaviour
{
    [Header("Projetil")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 25f;
    public float projectileLifetime = 5f;

    [Header("Cartucho")]
    public GameObject shellPrefab;
    public Transform shellEjectPoint;
    public float ejectForce = 2f;
    public float shellLifetime = 10f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            EjectShell();
        }

        
        if (Input.GetKeyUp(KeyCode.X))
        {
            Shoot();
            EjectShell();
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }
        Destroy(proj, projectileLifetime);
    }

    void EjectShell()
    {
        GameObject shell = Instantiate(shellPrefab, shellEjectPoint.position, shellEjectPoint.rotation);
        Rigidbody rb = shell.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplica força para o lado e um pouco para cima, para simular a ejeção
            Vector3 ejectDir = shellEjectPoint.right + shellEjectPoint.up * 0.5f;
            rb.AddForce(ejectDir * ejectForce, ForceMode.Impulse);

            // Um pouco de rotação aleatória
            rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
        }

        Destroy(shell, shellLifetime);
    }
}
