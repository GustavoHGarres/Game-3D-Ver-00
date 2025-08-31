using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cloth;

public class HealthBase : MonoBehaviour, IDamageable
{
    public float startLife = 10f;

    public bool destroyOnKill = false;

    [SerializeField] private float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public UIFillUpdater uiFillUpdater;

    public float damageMultiply = 1f;

    private void Awake()
    {
        Init();
    }


    public void Init()
    {
        ResetLife();
    }

    public void ResetLife()
    {
        _currentLife = startLife;
        UpdateUI(); //LIFE_pACK

    }

    protected virtual void Kill()
        { 
            if(destroyOnKill)
            Destroy(gameObject, 3f);

            if(OnKill != null) OnKill.Invoke(this); // OnKill?.Invoke();
        }

    [NaughtyAttributes.Button] 
    public void Damage()
    {
        Damage(5);
    }  

    public void Damage(float f)
    {
        _currentLife -= f * damageMultiply; // damageMultiply referencia ao power up da roupa invencivel.

        if(_currentLife <= 0)
        {
              Kill();
        }

        else
        {
            ShakeCamera.Instance.Shake();
        }
        
        UpdateUI();
        if(OnDamage != null) OnDamage.Invoke(this); // OnDamage?.Invoke();

        
    }

#region Interface para o dano no player

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
    }

#endregion

    private void UpdateUI()
    {
          if(uiFillUpdater != null)
          {
               uiFillUpdater.UpdateValue((float)_currentLife / startLife);
          }
    }

#region Integrando coletas de roupa invencivel

    public void ChangeDamageMultiply(float damage, float duration)
    {
         //StartCoroutine(ChangeDamageMultiplyCoroutine(damageMultiply, duration));
         StartCoroutine(ChangeDamageMultiplyCoroutine(damage, duration));
    }

    IEnumerator ChangeDamageMultiplyCoroutine(float damageMultiply, float duration)
    {
        this.damageMultiply = damageMultiply;
        yield return new WaitForSeconds(duration);
         this.damageMultiply = 1;       
    }

#endregion

}
