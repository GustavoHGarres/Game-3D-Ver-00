using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvencibilidade : MonoBehaviour
{
    public float duracaoInvencibilidade = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AtivarInvencibilidade(duracaoInvencibilidade);
                Debug.Log("Invencibilidade ativada por " + duracaoInvencibilidade + " segundos.");

                // Desaparece após coletado
                Destroy(gameObject);
            }
        }
    }
}
