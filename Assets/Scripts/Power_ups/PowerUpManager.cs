using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public float tempoParaAparecer = 120f; // Tempo em segundos (ex: 2 minutos)
    public List<GameObject> powerUpsParaAtivar;

    private void Start()
    {
        StartCoroutine(AtivarPowerUpsAposTempo());
    }

    IEnumerator AtivarPowerUpsAposTempo()
    {
        foreach (GameObject p in powerUpsParaAtivar)
            p.SetActive(false); // Garante que estejam ocultos

        yield return new WaitForSeconds(tempoParaAparecer);

        foreach (GameObject p in powerUpsParaAtivar)
            p.SetActive(true); // Ativa todos após o tempo
    }
}