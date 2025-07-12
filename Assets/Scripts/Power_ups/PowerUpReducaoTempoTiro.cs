using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpReducaoTempoTiro : MonoBehaviour
{
    public float novoTempoDeCarga = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TiroCarregado tiro = other.GetComponentInChildren<TiroCarregado>();
            if (tiro != null)
            {
                tiro.minChargeTime = novoTempoDeCarga;
            }
            Destroy(gameObject);
        }
    }
}