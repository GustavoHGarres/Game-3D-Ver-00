using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float maxChargeTime = 2f;
    public float maxForce = 20f;

    public GameObject chargeVisualPrefab; // Prefab do projétil visual
    public GameObject chargeVisualInstance;

    private float chargeTime = 0f;
    private bool isCharging = false;

    public float timeBetweenShoot = .3f;

     
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isCharging = true;
            chargeTime = 0f;
            StartChargeVisual();
        }

        if (isCharging && Input.GetButton("Fire1"))
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
            UpdateChargeVisual(chargeTime / maxChargeTime);
        }

        if (isCharging && Input.GetButtonUp("Fire1"))
        {
            FireChargedShot();
            isCharging = false;
            Destroy(chargeVisualInstance);
        }
    }


    protected virtual IEnumerator ShootCoroutine()
    {
         while(true)
        {
            FireChargedShot();
            yield return new WaitForSeconds(timeBetweenShoot);
        }
    }



   public void FireChargedShot()
    {
        float chargePercent = chargeTime / maxChargeTime;
        float force = chargePercent * maxForce;
        

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * force, ForceMode.Impulse);
       
      
    }

    void StartChargeVisual()
    {
        if (chargeVisualPrefab != null)
        {
            chargeVisualInstance = Instantiate(chargeVisualPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
    }

    void UpdateChargeVisual(float chargePercent)
    {
        if (chargeVisualInstance != null)
        {
            // Escala aumenta conforme o carregamento
            float scale = Mathf.Lerp(0.3f, 1.5f, chargePercent);
            chargeVisualInstance.transform.localScale = Vector3.one * scale;

            // Cor muda conforme o carregamento (vermelho → amarelo → branco)
            Renderer rend = chargeVisualInstance.GetComponent<Renderer>();
            if (rend != null)
            {
                Color color = Color.Lerp(Color.red, Color.white, chargePercent);
                rend.material.color = color;
                rend.material.SetColor("_EmissionColor", color * 2f); // Brilho
            }
        }
    }
}