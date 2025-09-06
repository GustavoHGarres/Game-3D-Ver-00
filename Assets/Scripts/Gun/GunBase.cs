using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    [Header("Gun Principal Setup")]
    public GameObject projectilePrefab;
    public Transform firePoint; 
    public float speed = 50f;
    public float timeBetweenShoot = .3f;

    private Coroutine _currentCoroutine;

    public Flashcolor _flashColor;
   
    

    public KeyCode keyCode = KeyCode.Z;

    public void Update()
    {
        if (Input.GetKeyDown(keyCode) || Input.GetButtonDown("Fire1"))
    {
        StartShoot(); // Começa a Coroutine controlada (com limite)
    }

    if (Input.GetKeyUp(keyCode) || Input.GetButtonUp("Fire1"))
    {
        StopShoot(); // Para de atirar quando solta o botão
    } 
    }



    protected virtual IEnumerator ShootCoroutine()
    {
         while(true)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShoot);
        }
    }


    public virtual void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);      
        proj.transform.position = firePoint.position;
        proj.transform.rotation = firePoint.rotation;      
        Vector3 direction = firePoint.forward;
        proj.GetComponent<ProjectileBase>().SetDirection(direction);
        ShakeCamera.Instance.Shake();
    }

    
    //Para parar a coroutine
    public virtual void StartShoot()
      {    
          StopShoot();
          _flashColor?.Flash();
          _currentCoroutine = StartCoroutine(ShootCoroutine());
      }

    public void StopShoot()
      {   
          if(_currentCoroutine != null)
           StopCoroutine(_currentCoroutine);
      }
    //Para parar a coroutine

   
}