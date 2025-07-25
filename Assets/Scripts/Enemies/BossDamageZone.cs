using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageZone : MonoBehaviour
{
    public int damageAmount = 1;
    public ParticleSystem ps;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (ps != null) ps.Play(); // Ativa partículas
            player.healthBase.Damage(damageAmount); // Causa dano
        }
    }
}