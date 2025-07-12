using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Invencibilidade,
    ReduzirCarga,
    ProjétilFogo,
    ProjétilGelo,
    ProjétilEletricidade
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType tipo;
    public float duracao = 5f;
    public float novoTempoDeCarga = 0.5f;
    public GameObject efeitoVisual;

    [Header("Grupo de Power-Up")]
    public string grupo; // Ex: "A" para Invencibilidade e ReduzirCarga | "B" para Fogo/Gelo/Eletricidade

    private bool foiColetado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (foiColetado) return;

        if (other.CompareTag("Player"))
        {
            foiColetado = true;
            AplicarEfeito(other.gameObject);
            RemoverOutrosDoGrupo();
            Destroy(gameObject);
        }
    }

    void AplicarEfeito(GameObject player)
    {
        var tiroCarregado = player.GetComponentInChildren<TiroCarregado>(true);
        var jogador = player.GetComponent<Player>();

        switch (tipo)
        {
            case PowerUpType.Invencibilidade:
                if (jogador != null)
                    jogador.AtivarInvencibilidade(duracao);
                break;

            case PowerUpType.ReduzirCarga:
                if (tiroCarregado != null)
                   tiroCarregado.ReduzirTempoCarga(novoTempoDeCarga, duracao);
                break;

            case PowerUpType.ProjétilFogo:
            case PowerUpType.ProjétilGelo:
            case PowerUpType.ProjétilEletricidade:
                if (tiroCarregado != null)
                    tiroCarregado.projectilePrefab = this.efeitoVisual;
                break;
        }
    }

    void RemoverOutrosDoGrupo()
    {
        foreach (var p in FindObjectsOfType<PowerUp>())
        {
            if (p != this && p.grupo == this.grupo)
                Destroy(p.gameObject);
        }
    }
}
