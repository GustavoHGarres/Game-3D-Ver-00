using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBigBossFinal : MonoBehaviour
{
    public GameObject bigBossFinal;
    private bool bossAtivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (bossAtivado) return;

        if (other.CompareTag("Player"))
        {
            bossAtivado = true;
            bigBossFinal.SetActive(true);

            Boss.BossBase boss = bigBossFinal.GetComponent<Boss.BossBase>();
            if (boss != null)
            {
                boss.SwitchState(Boss.BossAction.INIT);
            }
        }
    }
}
