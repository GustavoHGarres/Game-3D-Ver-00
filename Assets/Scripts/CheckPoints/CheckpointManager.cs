using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class CheckpointManager : Singleton<CheckpointManager>
{
   public int lastCheckPointKey = 0;

   public List<CheckpointBase> checkpoints;

   public bool HasCheckpoint() // Checkpoint de save do player 
   {
          return lastCheckPointKey > 0;
   }


   public void SaveCheckpoint(int i)
   {
          if(i > lastCheckPointKey)
          {
               lastCheckPointKey = i;
          }

   }

   public Vector3 GetPositionFromLastChecpoint()
   {
    var checkpoint = checkpoints.Find(i => i.key == lastCheckPointKey);

    if (checkpoint != null)
    {
        Transform spawnPoint = checkpoint.transform.Find("SpawnPoint");
        if (spawnPoint != null)
        {
            return spawnPoint.position;
        }

    }

    return Vector3.zero;
   }
}
