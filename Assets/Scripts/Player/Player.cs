using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using Cloth;

public class Player : Singleton<Player> //, IDamageable
{
    [Header("Player Setup")]
    public CharacterController characterController;
    public Animator animator;
    public List<Collider> colliders; // Quando o player morre desativa o box collider
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

    [Header("Life")]
    public HealthBase healthBase;

    [Space]
    [SerializeField] private ClothChanger _clothChanger;

    private bool _alive = true; // Atribui animacao de morte ao player 
    private bool _jumping = false;

    [Header("VFX")]
    public ParticleSystem dustParticles;

    // Atribui dano ao player sem o uso da interface IDamageable
    public void OnValidate()
    {
        //if(healthBase != null) healthBase = GetComponent<HealthBase>();
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    protected override void Awake()
    {
        base.Awake();
        OnValidate();

        healthBase.OnDamage += Damage; // Atribui dano ao player com flashcolors
        healthBase.OnKill += OnKill; // Atribui animacao de morte ao player 

    }

    private void Start()
    {
        if (playerRenderer != null)
        {
            corEmissionOriginal = playerRenderer.material.GetColor("_EmissionColor");
        }
    }

#region Life

         public void OnKill(HealthBase h)
         {
             if(_alive)
             {
                _alive = false;
                animator.SetTrigger("Death");
                colliders.ForEach(i => i.enabled = false);

                Invoke(nameof(Revive), 3f); // Checkpoint de save do player 
             }            
         }

         private void Revive() // Checkpoint de save do player 
         {
              _alive = true;
              healthBase.ResetLife();
              animator.SetTrigger("Revive");
              Respawn();
              Invoke(nameof(TurnOnColliders), .1f);
         }

         private void TurnOnColliders()
         {
             colliders.ForEach(i => i.enabled = true);
         }

        public void Damage(HealthBase h)
        {
              flashcolors.ForEach(i => i.Flash());
              EffectsManager.Instance.ChangeVignette();
        }

        public void Damage(float damage, Vector3 dir)
        {
              healthBase.Damage(damage); //Lembra?
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

            if (_jumping)
            {
                _jumping = false;
                animator.SetTrigger("Land");
            }

            vSpeed = 0;
            if (Input.GetKeyDown(jumpKeyCode))
            {
                vSpeed = jumpSpeed;

                if (!_jumping)
                {
                    _jumping = true;
                    animator.SetTrigger("Jump");
                } 
            } 

            // Ativar poeira apenas quando estiver correndo
            if (inputAxisVertical != 0 && Input.GetKey(keyRunCode))
            {
                if (!dustParticles.isPlaying)
                dustParticles.Play();
            }
                else
                {
                    if (dustParticles.isPlaying)
                    dustParticles.Stop();
                }                  
        }

        vSpeed -= gravity * Time.deltaTime;
        speedVector.y = vSpeed;
        characterController.Move(speedVector * Time.deltaTime);

        // Animacao
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

    [NaughtyAttributes.Button]
    public void Respawn() // Checkpoint de save do player 
    {
        if (CheckpointManager.Instance.HasCheckpoint())
           {
               transform.position = CheckpointManager.Instance.GetPositionFromLastChecpoint();
           }
    }

#region Integrando coletas de roupa que aumenta a velocidade

    public void ChangeSpeed(float speed, float duration)
    {
         StartCoroutine(ChangeSpeedCoroutine(speed, duration));
    }

    IEnumerator ChangeSpeedCoroutine(float localSpeed, float duration)
    {
        var defaultSpeed = speed;
        speed = localSpeed;
        yield return new WaitForSeconds(duration);
        speed = defaultSpeed;

    }

    public void ChangeTexture(ClothSetup setup, float duration)
    {
         StartCoroutine(ChangeTextureCoroutine(setup, duration));
    }

    IEnumerator ChangeTextureCoroutine(ClothSetup setup, float duration)
    {
        _clothChanger.ChangeTexture(setup);  

        if (duration <= 0f) yield break; // SKIN permanente - Integrando coleta da Skin

        yield return new WaitForSeconds(duration);
        _clothChanger.ResetTexture();       
    }

#endregion    
}
