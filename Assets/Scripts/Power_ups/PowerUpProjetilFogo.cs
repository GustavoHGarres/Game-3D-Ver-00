using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpProjetilFogo : MonoBehaviour
{
    public GameObject novoProjectilePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TiroCarregado tiro = other.GetComponentInChildren<TiroCarregado>();
            if (tiro != null)
            {
                tiro.projectilePrefab = novoProjectilePrefab;
            }
            Destroy(gameObject);
        }
    }
}