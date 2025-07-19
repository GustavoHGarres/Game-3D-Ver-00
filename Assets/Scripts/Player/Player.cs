using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
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

    [Header("Invencibilidade")]
    public bool invencivel = false;
    public float tempoInvencivel = 5f;
    public Renderer playerRenderer; // arraste o SkinnedMeshRenderer no Inspector
    public Color corInvencivel = new Color(1f, 1f, 0f) * 5f; // Amarelo com brilho HDR
    private Color corEmissionOriginal;

    private float vSpeed = 0f;
    
    [Header("Flash")]
    public List<Flashcolor> flashcolors;

    void Start()
    {
        if (playerRenderer != null)
        {
            corEmissionOriginal = playerRenderer.material.GetColor("_EmissionColor");
        }
    }
#region Life

        public void Damage(float damage)
        {
              flashcolors.ForEach(i => i.Flash());
        }

        public void Damage(float damage, Vector3 dir)
        {

        }

#endregion

    void Update()
    {
        // Movimento
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = transform.forward * inputAxisVertical * speed;

        // Pulo
        if (characterController.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown(jumpKeyCode))
                vSpeed = jumpSpeed;
        }

        vSpeed -= gravity * Time.deltaTime;
        speedVector.y = vSpeed;
        characterController.Move(speedVector * Time.deltaTime);

        // Animação
        animator.SetBool("Run", inputAxisVertical != 0);

        // Corrida
        if (inputAxisVertical != 0)
        {
            if (Input.GetKey(keyRunCode))
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

    public void AtivarInvencibilidade(float duracao)
    {
        if (!invencivel)
            StartCoroutine(InvencibilidadeCoroutine(duracao));
    }

    private IEnumerator InvencibilidadeCoroutine(float duracao)
    {
        invencivel = true;

        if (playerRenderer != null)
        {
            // Ativa o emissivo
            playerRenderer.material.EnableKeyword("_EMISSION");
            playerRenderer.material.SetColor("_EmissionColor", corInvencivel);
        }

        yield return new WaitForSeconds(duracao);

        invencivel = false;

        if (playerRenderer != null)
        {
            playerRenderer.material.SetColor("_EmissionColor", corEmissionOriginal);
            playerRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
