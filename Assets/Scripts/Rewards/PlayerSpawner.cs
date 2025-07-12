using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform pontoSpawn;         // Onde o jogador vai nascer
    public GameObject defaultPrefab;     // Visual padrão, se nenhum foi salvo

    private void Start()
    {
        string visualName = PlayerPrefs.GetString("VISUAL_SELECIONADO", "");
        GameObject prefabParaInstanciar = defaultPrefab;

        if (!string.IsNullOrEmpty(visualName))
        {
            GameObject visualPrefab = Resources.Load<GameObject>(visualName);
            if (visualPrefab != null)
            {
                prefabParaInstanciar = visualPrefab;
                Debug.Log("Instanciando visual: " + visualName);
            }
            else
            {
                Debug.LogWarning("Visual salvo não encontrado em Resources: " + visualName);
            }
        }

        if (prefabParaInstanciar != null && pontoSpawn != null)
        {
            Instantiate(prefabParaInstanciar, pontoSpawn.position, pontoSpawn.rotation);
        }
        else
        {
            Debug.LogWarning("PlayerSpawner: Visual ou ponto de spawn não atribuído!");
        }
    }
}
