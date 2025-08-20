using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class DestructableItem : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private HealthBase healthBase;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform dropPosition;

    [Header("Feedback")]
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private int shakeForce = 1;

    [Header("Limites")]
    [Tooltip("Máximo total de moedas que este objeto pode soltar (dano + kill).")]
    [SerializeField] private int maxTotalCoins = 12;

    [Header("Moedas por Dano")]
    [SerializeField] private bool dropOnDamage = true;
    [SerializeField] private int coinsPerHitMin = 2;
    [SerializeField] private int coinsPerHitMax = 2;
    [SerializeField] private float hitCooldown = 0.08f;

    [Header("Moedas no Kill")]
    [SerializeField] private bool dropOnKill = true;
    [SerializeField] private int coinsOnKillMin = 6;
    [SerializeField] private int coinsOnKillMax = 6;
    [SerializeField] private float killDropInterval = 0.05f;

    [Header("Espalhamento / Física (opcional)")]
    [SerializeField] private float spreadRadius = 0.6f;
    [SerializeField] private float upwardOffset = 0.1f;
    [SerializeField] private float impulse = 2.2f;
    [SerializeField] private float torque = 18f;

    private int coinsDropped = 0;
    private bool canDropOnHit = true;

    private void OnValidate()
    {
        if (!healthBase) healthBase = GetComponent<HealthBase>();
        if (!dropPosition) dropPosition = transform;
        coinsPerHitMin = Mathf.Max(0, coinsPerHitMin);
        coinsPerHitMax = Mathf.Max(coinsPerHitMin, coinsPerHitMax);
        coinsOnKillMin = Mathf.Max(0, coinsOnKillMin);
        coinsOnKillMax = Mathf.Max(coinsOnKillMin, coinsOnKillMax);
        maxTotalCoins = Mathf.Max(0, maxTotalCoins);
    }

    private void Awake()
    {
        OnValidate();
        if (healthBase)
        {
            healthBase.OnDamage += HandleDamage;
            healthBase.OnKill += HandleKill;
        }
    }

    private void OnDestroy()
    {
        if (healthBase)
        {
            healthBase.OnDamage -= HandleDamage;
            healthBase.OnKill -= HandleKill;
        }
    }

    private void HandleDamage(HealthBase h)
    {
        transform.DOShakeScale(shakeDuration, Vector3.up * 0.5f, shakeForce);

        if (!dropOnDamage || !canDropOnHit || coinsDropped >= maxTotalCoins) return;

        canDropOnHit = false;
        int qty = Random.Range(coinsPerHitMin, coinsPerHitMax + 1);
        qty = Mathf.Min(qty, maxTotalCoins - coinsDropped); // respeita limite
        StartCoroutine(DropBurst(qty));
        StartCoroutine(HitCooldownCo());
    }

    private void HandleKill(HealthBase h)
    {
        if (!dropOnKill || coinsDropped >= maxTotalCoins) return;

        int qty = Random.Range(coinsOnKillMin, coinsOnKillMax + 1);
        qty = Mathf.Min(qty, maxTotalCoins - coinsDropped);
        StartCoroutine(DropSequential(qty, killDropInterval));
    }

    private IEnumerator HitCooldownCo()
    {
        yield return new WaitForSeconds(hitCooldown);
        canDropOnHit = true;
    }

    private IEnumerator DropBurst(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnCoin();
            yield return null; // levemente desenfileirado
        }
    }

    private IEnumerator DropSequential(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnCoin();
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnCoin()
    {
        if (!coinPrefab || coinsDropped >= maxTotalCoins) return;

        var coin = Instantiate(coinPrefab);
        var basePos = dropPosition ? dropPosition.position : transform.position;

        // posição com espalhamento
        Vector3 rand = Random.insideUnitSphere; rand.y = 0f;
        Vector3 spawnPos = basePos + Vector3.up * upwardOffset + rand.normalized * Random.Range(0f, spreadRadius);
        coin.transform.position = spawnPos;

        // entrada bonita
        coin.transform.localScale = Vector3.zero;
        coin.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

        // física opcional
        if (coin.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 dir = (coin.transform.position - basePos).normalized + Vector3.up * 0.6f;
            rb.AddForce(dir * impulse, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * torque, ForceMode.Impulse);
        }

        coinsDropped++;
    }

    // Botões de teste (se usar NaughtyAttributes)
    [NaughtyAttributes.Button] private void TestHitDrop()  => StartCoroutine(DropBurst(Mathf.Min(3, maxTotalCoins - coinsDropped)));
    [NaughtyAttributes.Button] private void TestKillDrop() => StartCoroutine(DropSequential(Mathf.Min(6, maxTotalCoins - coinsDropped), killDropInterval));
}