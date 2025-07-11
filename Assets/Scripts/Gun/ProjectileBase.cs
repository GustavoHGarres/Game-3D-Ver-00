using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    public float timeToDestroy = 2f;

    public int damageAmount = 1;

    private void Awake()
   {
       Destroy(gameObject, timeToDestroy);
   }

    // Define a direção do projétil
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // Move o projétil na direção definida
        transform.position += direction * speed * Time.deltaTime;
    }

    //Causa dano no inimigo
     private void OnCollisionEnter(Collision collision)
     {
         var damageable = collision.transform.GetComponent<IDamageable>();

         if (damageable != null) damageable.Damage(damageAmount); 
          {
             //Vector3 dir = collision.transform.position - transform.position; //// Quando recebe impacto do projetil desloca o inimigo para frente;
             //dir = -dir.normalized;
             //dir.y = 0; // Inimigo permanece na mesma posição;
             Destroy(gameObject); // Destroy a bala;
             //damageable.Damage(damageAmount, dir);
          }
          //Destroy(gameObject); // Destroy a bala;

   }

}


