using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartCheck : MonoBehaviour
{
   public string tagToPlayer = "Player"; 
   public GameObject bossCamera;
   public Color gizmoColor = Color.yellow;

   private void Awake()
   {
        bossCamera.SetActive(false);
   }
     
   void OnTriggerEnter(Collider other)
   {
       if(other.transform.tag == tagToPlayer)
       {
           TurnCameraOn();
       }
   }

   private void TurnCameraOn()
   {
        bossCamera.SetActive(true);
   }

   private void OnDrawGizmos()
   {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, transform.localScale.y); 
   }
}
