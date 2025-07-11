using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animation;

namespace Enemy
{
   public class EnemyBase : MonoBehaviour, IDamageable
   {
       public Collider collider; //Nao precisa de colisao depois que o inimigo morreu

       public Flashcolor flashcolor;

       public ParticleSystem particleSystem;

       [SerializeField] private float dissolveDuration = 1.5f; //Animacao de dissolver o geleia

       public float startLife = 10f;
       [SerializeField] private float _currentLife;

       [Header("Animation")]
       [SerializeField] private AnimationBase _animationBase;

       [Header("Start Animation")]
       public float startAnimationDuration = .2f;
       public Ease startAnimationEase = Ease.OutBack;
       public bool startWithBornAnimation = true; // Controla se comeca ou nao com animacao de escala

       private void Awake()
       {
            Init();
       }
        
       public void Start()
       {
           //GameManager.Instance.RegistrarInimigo(); // Confirme que todos os inimigos estao chamando RegistrarInimigo() ao nascer
       }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        protected virtual void Init()
        {
            ResetLife();
            if(startWithBornAnimation)
            BornAnimation();

            PlayAnimationByTrigger(AnimationType.ATTACK); // Para a tarefa, fazer o inimigo atacar

            GameManager.Instance.RegistrarInimigo(); //Integracao de transicao de cena
        }


        protected virtual void Kill()
        {
           OnKill();
        }

        protected virtual void OnKill()
        { 
            if(collider != null) collider.enabled = false; //Nao precisa de colisao depois que o inimigo morreu
            //Destroy(gameObject, 3f);
            PlayAnimationByTrigger(AnimationType.DEATH);
            StartCoroutine(DissolveAndDestroy()); //Animacao de dissolver o geleia

            GameManager.Instance.InimigoMorto(); //Integracao de transicao de cena
        }

        // Dano no inimigo
        public void OnDamage(float f)
        {
              if(flashcolor != null) flashcolor.Flash();
              if(particleSystem != null) particleSystem.Emit(15);
              transform.position -= transform.forward; // Quando recebe impacto do projetil desloca o inimigo para frente;
              _currentLife -= f;

              if(_currentLife <= 0)
              {
              Kill();
              }
        }
        // Dano no inimigo

#region Animation escala do inimigo e animacaodo inimigo

       private void BornAnimation()
       {
           transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From(); // O From indica que vai comecar do 0
       }

       public void PlayAnimationByTrigger(AnimationType animationType)
       {
           _animationBase.PlayAnimationByTrigger(animationType);
       }

#endregion

        public void Update()
        {
            
            if(Input.GetKeyDown(KeyCode.F))           
           {                
              OnDamage(5f);    
           } 
        }

#region Implemento de interfaces, serve para dar danos em todos os inimigos sem target

        public void Damage(float damage)
        {
            // throw new System.NotImplementedException(); //Lembra o implemento da interface
            Debug.Log("Damage");
            OnDamage(damage);
        }

#endregion   

#region Implemento para o inimigo dissolver com a animacao de morte
        
        private IEnumerator DissolveAndDestroy()
        {
            MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
            SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
            Material mat = null;

            if (mr != null)
            mat = mr.material = new Material(mr.material); // Isso duplica o material so pro inimigo que vai morrer, evitando que todos fiquem transparentes junto com ele
            else if (smr != null)
            mat = mr.material = new Material(smr.material); // Isso duplica o material so pro inimigo que vai morrer, evitando que todos fiquem transparentes junto com ele

            if (mat != null)
            {
                Color cor = mat.color;

                float timer = 0f;
                while (timer < dissolveDuration)
                {
                    float alpha = Mathf.Lerp(1f, 0f, timer / dissolveDuration);
                    mat.color = new Color(cor.r, cor.g, cor.b, alpha);
                    timer += Time.deltaTime;
                    yield return null;
                }

                   mat.color = new Color(cor.r, cor.g, cor.b, 0f);
            }

                  Destroy(gameObject);
        }
        
#endregion

   }
}
