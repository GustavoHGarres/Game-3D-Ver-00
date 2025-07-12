using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public float tempoMin = 3f;
    public float tempoMax = 5f;

    void Start()
    {
        StartCoroutine(SpawnUnicoPowerUp());
    }

    IEnumerator SpawnUnicoPowerUp()
    {
        float tempoEspera = Random.Range(tempoMin, tempoMax);
        yield return new WaitForSeconds(tempoEspera);

        Instantiate(powerUpPrefab, transform.position, transform.rotation);
        Destroy(gameObject); // remove o spawner após spawnar 1x (opcional)
    }
}