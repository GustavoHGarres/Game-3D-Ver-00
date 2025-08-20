using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    [Header("Alvo (opcional)")]
    public Transform target;              // se nao setar, tenta Player.Instance ou tag "Player"

    [Header("Distancia minima para parar")]
    public float minDistance = 0.25f;

    [Header("Velocidade")]
    public float startSpeed = 3f;
    public float acceleration = 10f;
    public float maxSpeed = 20f;

    [Header("Delay para comecar (moeda 'aparece', depois puxa)")]
    public float startDelay = 0.1f;

    private float _speed;
    private float _timer;
    private bool _active;

    void OnEnable()
    {
        _speed = startSpeed;
        _timer = 0f;
        _active = false;
    }

    void Update()
    {
        if (!_active)
        {
            _timer += Time.deltaTime;
            if (_timer >= startDelay) _active = true;
            else return;
        }

        Transform t = target;
        if (t == null)
        {
            // tenta Player.Instance
            if (Player.Instance != null) t = Player.Instance.transform;
            else
            {
                // fallback por tag
                var go = GameObject.FindGameObjectWithTag("Player");
                if (go != null) t = go.transform;
            }
            if (t == null) return; // sem player ainda
        }

        Vector3 to = t.position - transform.position;
        float dist = to.magnitude;
        if (dist <= minDistance) return;

        _speed = Mathf.Min(maxSpeed, _speed + acceleration * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, t.position, _speed * Time.deltaTime);
    }
}