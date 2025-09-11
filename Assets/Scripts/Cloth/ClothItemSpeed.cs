using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{

     public class ClothItemSpeed : ClothItemBase
     {
          public float targetSpeed = 2f;

          public override void Collect()
          {
               base.Collect();
               Player.Instance.ChangeSpeed(targetSpeed, duration);

               // registra no save que a última roupa coletada foi SPEED
               SaveManager.Instance.SetOutfit(Cloth.ClothType.SPEED.ToString());
          }
     }

     

}
