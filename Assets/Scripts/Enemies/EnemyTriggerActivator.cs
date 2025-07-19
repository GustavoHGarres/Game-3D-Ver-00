using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerActivator : MonoBehaviour
{
    public GameObject enemyToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyToActivate.SetActive(true);
        }
    }
}