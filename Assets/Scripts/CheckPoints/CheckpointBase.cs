using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBase : MonoBehaviour
{

   public MeshRenderer meshRenderer;

   public int key = 1;
   private string checkpointKey = "CheckpointKey";

   private bool checkpointActived = false;

   private void OnTriggerEnter(Collider other)
   {
       if(!checkpointActived && other.transform.tag == "Player")
       {
           CheckCheckpoint();
       }
       
   }

   private void CheckCheckpoint()
   {
       TurnItOn();
       SaveCheckpoint();
   }
   
   [NaughtyAttributes.Button]
   private void TurnItOn()
   {
       meshRenderer.material.SetColor("_EmissionColor", Color.white);
   }

   
   [NaughtyAttributes.Button]
   private void TurnItOff()
   {
       meshRenderer.material.SetColor("_EmissionColor", Color.grey);
   }

   private void SaveCheckpoint()
   {
       //if(PlayerPrefs.GetInt(checkpointKey, 0) > key)
         //PlayerPrefs.SetInt(checkpointKey, key);
         
         CheckpointManager.Instance.SaveCheckpoint(key);
         
         checkpointActived = true;
   }
}
