using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;

public class FSMExample : MonoBehaviour
{
   public enum ExampleEnum
   {
    STATE_ONE,
    STATE_TWO,
    STATE_THREE
   }

   public StateMachine<ExampleEnum> stateMachine;

   private void Start()
   {
    stateMachine = new StateMachine<ExampleEnum>();  
    //stateMachine = new StateMachine<ExampleEnum>(ExampleEnum.STATE_ONE);
    stateMachine.Init();
    stateMachine.RegisterStates(ExampleEnum.STATE_ONE, new StateBase());
    stateMachine.RegisterStates(ExampleEnum.STATE_TWO, new StateBase());
   }
}
