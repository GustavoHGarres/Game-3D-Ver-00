using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;
using DG.Tweening;

namespace Boss
{
        public enum BossAction
        {
             INIT,
             IDLE,
             WALK,
             ATTACK,
             DEATH
        }


        public class BossBase : MonoBehaviour
        {

            [Header("Animation")]
            public float startAnimationDuration =  .5f;
            public Ease startAnimationEase = Ease.OutBack;

            
            [Header("Attack")]
            public int attackAmount = 5;
            public float timeBetweenAttacks = .5f;

            public float speed = 5f;
            public List<Transform> waypoints;

            public HealthBase healthBase;

            private StateMachine<BossAction> stateMachine;

           public Player _player; //Inimigo olhar para o player
           public bool lookAtPlayer = false;
             

             private void Awake()
             {
                Init();
                healthBase.OnKill += OnBossKill; // Integrando vida do Boss
             }

#region Integra o inimigo e o boss aparecendo apos um tempo e animacoes a

             private void Start()
             {
                  //player = GameObject.FindObjectOfType<Player>().transform; // Olhar o player
                  _player = GameObject.FindObjectOfType<Player>(); //Inimigo olhar para o player

                   SwitchState(BossAction.INIT);

                  // Espera 1s e ja entra no Walk
                  Invoke(nameof(StartWalkAutomatically), 1f);
             }

             private void Update()
             {
                 LookAtPlayer();
             }

             private void StartWalkAutomatically()
             {
                   SwitchState(BossAction.WALK);
             }

#endregion             

             private void Init()
             {
                stateMachine = new StateMachine<BossAction>();
                stateMachine.Init();

                stateMachine.RegisterStates(BossAction.INIT, new BossStateInit());
                stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
                stateMachine.RegisterStates(BossAction.ATTACK, new BossStateAttack());
                stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());               
             }

             // Integrando vida do Boss
             private void OnBossKill(HealthBase h)
             {
                  SwitchState(BossAction.DEATH);
             }

#region Walk
            [NaughtyAttributes.Button]
            public void GoToRandomPoint(Action onArrive = null)
            {
                 StartCoroutine(GotoPointCoroutine(waypoints[UnityEngine.Random.Range(0, waypoints.Count)], onArrive));
            }

            IEnumerator GotoPointCoroutine(Transform t, Action onArrive = null)
            {
                while(Vector3.Distance(transform.position, t.position) > 1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, t.position, Time.deltaTime * speed);
                    yield return new WaitForEndOfFrame();
                }
                if(onArrive != null) onArrive.Invoke(); // Ou onArrive?.Invoke();
            }

#endregion

#region Attack
      
            public void StartAttack(Action endCallback = null)
            {
                 StartCoroutine(AttackCoroutine(endCallback));
            }

            IEnumerator AttackCoroutine(Action endCallback)
            {
                int attacks = 0;
                while(attacks < attackAmount)
                {
                    attacks++;
                    transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                    yield return new WaitForSeconds(timeBetweenAttacks);
                }

                if(endCallback != null) endCallback.Invoke(); // Ou endCallBack?.Invoke();

            }

#endregion

#region Animation
        
        public void StartInitAnimation()
        {
            gameObject.SetActive(true); // Garante que esteja ativo - Integracao do Big Boss Final
            transform.localScale = Vector3.one; // Corrige a escala final - Integracao do Big Boss Final
            transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From();
        }

#endregion



#region Debug
             [NaughtyAttributes.Button]
             private void SwitchInit()
             {
                SwitchState(BossAction.INIT);
             }

             [NaughtyAttributes.Button]
             private void SwitchWalk()
             {
                SwitchState(BossAction.WALK);
             }

             [NaughtyAttributes.Button]
             private void SwitchAttack()
             {
                SwitchState(BossAction.ATTACK);
             }

#endregion


#region State Nachine
        
       public void SwitchState(BossAction state)
       {
             stateMachine.SwitchState(state, this);
       }

#endregion   

#region Update

private void LookAtPlayer()
{
    if (lookAtPlayer && _player != null)
    {
        Vector3 direction = _player.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        }
    }
}

#endregion

        } 


}           
