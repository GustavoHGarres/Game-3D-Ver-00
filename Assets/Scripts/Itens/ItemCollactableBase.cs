using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Itens
{
public class ItemCollactableBase : MonoBehaviour
{
    public ItemType itemType;

    public string comparetag = "Player";
    public ParticleSystem particleSystem;
    public float timeToHide = 3;
    public GameObject graphicItem;

    public Collider collider;
    private bool _collected = false;

    [Header("Sounds")]
    public AudioSource audioSource;

    public void Awake()
    {
       // if (particleSystem != null) particleSystem.transform.SetParent(null);
    }
        
        private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.CompareTag(comparetag))
       {
         Collect();
       }
    }
    
    // Aula 3 modulo 35. Na aula conta as moedas de 2 em 2 mesmo com o colider desativado
       //protected virtual void Collect()
       //{
       // if(collider != null) collider.enabled = false;
       //if (graphicItem != null) graphicItem.SetActive(false);
       //Invoke("HideObject", timeToHide);
       //OnCollect();
      //}  
    // 
    
    protected virtual void Collect()
    {
         if (_collected) return;
         _collected = true;

         if (collider != null) collider.enabled = false;
         if (graphicItem != null) graphicItem.SetActive(false);
    
         OnCollect();
         Invoke("HideObject", timeToHide);
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnCollect()
    {
        if(particleSystem != null) particleSystem.Play();
        if (audioSource != null) audioSource.Play();
        ItemManager.Instance.AddByType(itemType);
    }
  
}
}
