using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour, IDamageable
{
    public float startLife = 10f;

    public bool destroyOnKill = false;

    [SerializeField] private float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public UIFillUpdater uiFillUpdater;

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
        _currentLife -= f;

        if(_currentLife <= 0)
        {
              Kill();
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

}
