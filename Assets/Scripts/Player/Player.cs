using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
      [Header("Player Setup")]
      public CharacterController characterController; 
      public Animator animator;
      public float speed = 1f;   
      public float turnSpeed = 1f;    
      public float gravity = 9.8f;    
      
      [Header("Jump Setup")]
      public float jumpSpeed = 15f;
      public KeyCode jumpKeyCode = KeyCode.Space;

      [Header("Run Setup")]   
      public KeyCode keyRunCode = KeyCode.LeftShift;    
      public float speedRun = 1.5f;



      private float vSpeed = 0f;  

      void Update()    
      {       

        //Movimento do personagem - CharacterController
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);        
        var inputAxisVertical = Input.GetAxis("Vertical");        
        var speedVector = transform.forward * inputAxisVertical * speed;    

        //Pulo
        if(characterController.isGrounded)        
        {            
           vSpeed = 0;            
            if(Input.GetKeyDown(jumpKeyCode)) 
            //if(Input.GetKeyDown(KeyCode.Space))           
           {                
              vSpeed = jumpSpeed;            
           }        
        }
        //Pulo

        vSpeed  -= gravity * Time.deltaTime;        
        speedVector.y = vSpeed;   

        characterController.Move(speedVector * Time.deltaTime);    
        //Movimento do personagem - CharacterController

        //Amimator
          if(inputAxisVertical !=0)       
          {             
            animator.SetBool("Run", true);       
          }       
          
          else       
          {             
            animator.SetBool("Run", false);        
          }
        //Amimator

        //Faz o personagem correr
        var isWalking = inputAxisVertical != 0;        
        if(isWalking)        
          {            
              if(Input.GetKey(keyRunCode))            
                 {                
                     speedVector *= speedRun;                
                     animator.speed = speedRun;
                 }
 
            else            
                 {                
                    animator.speed = 1;            
                 }        
          }

     }
}