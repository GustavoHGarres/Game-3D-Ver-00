using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    public float timeToDestroy = 2f;

    public int damageAmount = 1;

    public List<string> tagsToHit;

    private void Awake()
   {
       Destroy(gameObject, timeToDestroy);
   }

    // Define a direcao do projetil
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // Move o projetil na direcao definida
        transform.position += direction * speed * Time.deltaTime;
    }

    //Causa dano no inimigo
     private void OnCollisionEnter(Collision collision)
     {
        foreach(var tag in tagsToHit)
        {
             if(collision.transform.tag == tag)  
             {
                var damageable = collision.transform.GetComponent<IDamageable>();

                if (damageable != null) damageable.Damage(damageAmount); 
                {
                     Vector3 dir = collision.transform.position - transform.position; // Quando recebe impacto do projetil desloca o inimigo para frente;
                     dir = -dir.normalized; // Normaliza numeros quebrados - 0,0035 fica 0,001
                     dir.y = 0; // Inimigo permanece na mesma posicao;
                     Destroy(gameObject); // Destroy a bala;
                     damageable.Damage(damageAmount, dir);
                }  

                break;
            }   
        }
     }   

}


