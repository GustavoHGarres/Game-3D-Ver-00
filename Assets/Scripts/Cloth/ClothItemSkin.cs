using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{

      public class ClothItemSkin : ClothItemBase
      {
        [Header("Se tempo for 0, a Skin fica permanente")]
        [Tooltip("Duracao da skin em segundos. Se 0, fica permanente.")]
        public float durationSkin = 2f;

        public override void Collect()
        {
            // se for 0 → permanente
            duration = durationSkin <= 0 ? 0f : durationSkin;
            base.Collect(); // chama Player.ChangeTexture(setup, duration)
        }
      }
}
