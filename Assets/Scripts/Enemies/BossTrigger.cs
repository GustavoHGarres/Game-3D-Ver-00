using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private bool _activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            _activated = true;
            Boss.BossBase boss = FindObjectOfType<Boss.BossBase>();
            boss.SwitchState(Boss.BossAction.INIT);
        }
    }
}
