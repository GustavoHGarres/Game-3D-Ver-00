using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShootLimit : GunBase
{

   [Header("GunShootLimit Setup")]
   public float maxShoot = 5f;
   public float timeToRecharge = 1f;
   private float _currentShoots;
   private bool _recharging = false;

   public UIGunLimit gunUI;
   private float reloadProgress = 0f; // Slider

  
   protected override IEnumerator ShootCoroutine()
   {
       if (_recharging) yield break;

       while(true)
       {
          if(_currentShoots < maxShoot)
          {
            Shoot();
            _currentShoots++;
            gunUI?.UpdateAmmo((int)_currentShoots, (int)maxShoot);
            CheckRecharge();
          }
          yield return new WaitForSeconds(timeBetweenShoot);
       }
   }

   private void CheckRecharge()
   {
       if (_currentShoots >= maxShoot)
       {
          StopShoot();
          StartRecharge();
       }
   }

   private void StartRecharge()
   {
       _recharging = true;
       StartCoroutine(RechargeCoroutine());
       gunUI?.SetStatus("Recarregando...");
       gunUI?.ShowReloadBar(true); // Slider
   }

   IEnumerator RechargeCoroutine()
    {
        float time = 0;
        while (time < timeToRecharge)
        {
           time += Time.deltaTime;
           float progress = Mathf.Clamp01(time / timeToRecharge);
           gunUI?.UpdateReloadBar(progress); // Slider
           yield return null;
        }

        _currentShoots = 0;
        _recharging = false;

        gunUI?.SetStatus("Pronto!");
        gunUI?.UpdateAmmo(0, (int)maxShoot);
        gunUI?.ShowReloadBar(false); //Slider
     }
     
     //Override para chamar a UI
     public override void StartShoot()
     {
        if (_recharging) return; // Não pode atirar se estiver recarregando

        base.StartShoot(); // Chama a versão da classe base que inicia a coroutine de tiro

        // Atualiza a UI para mostrar que está atirando
        gunUI?.SetStatus("Atirando...");
     }
     //Override para chamar a UI
}
