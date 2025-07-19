using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;

namespace Boss
{

        public class BossStateBase : StateBase
        {
              protected BossBase boss;

              public override void OnStateEnter(params object[] objs)
              {
              base.OnStateEnter(objs);
              boss = (BossBase)objs[0];
              }
        }

        public class BossStateInit : BossStateBase
        {
              public override void OnStateEnter(params object[] objs)
              {
                   base.OnStateEnter(objs);
                   boss.StartInitAnimation();

                   boss.Invoke(nameof(GotoWalk), boss.startAnimationDuration);

                   Debug.Log("Boss: " + boss);
              }

               public void GotoWalk()
              {
                   boss.SwitchState(BossAction.WALK);
              }  
        }
        
                 
              

        public class BossStateWalk : BossStateBase
        {
              public override void OnStateEnter(params object[] objs)
              {
                   base.OnStateEnter(objs);
                   boss.GoToRandomPoint(OnArrive);
                   Debug.Log("Boss: " + boss);
              }

              private void OnArrive()
              {
                   boss.SwitchState(BossAction.ATTACK);
              }

              public override void OnStateExit()
              {
                   base.OnStateExit();
                   boss.StopAllCoroutines();
              }
        }

        public class BossStateAttack : BossStateBase
        {
              public override void OnStateEnter(params object[] objs)
              {
                   base.OnStateEnter(objs);
                   boss.StartAttack(EndAttacks); // Chama a funcao no fim do ataque
                   //boss.StartAttack();
                   
                   Debug.Log("Boss: " + boss);
              }

              private void EndAttacks()
              {
                   boss.SwitchState(BossAction.WALK); // Volta a andar apos atacar
              }
        }

        public class BossStateDeath : BossStateBase
        {
            public override void OnStateEnter(params object[] objs)
            {
                   base.OnStateEnter(objs);
                   boss.transform.localScale = Vector3.one * .2f;
            }

        }
}