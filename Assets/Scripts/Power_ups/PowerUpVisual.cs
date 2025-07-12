using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpVisual : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    public Renderer visualRenderer;
    public float glowFrequency = 2f;
    public float glowIntensity = 2f;

    private Vector3 startPosition;
    private Material material;

    void Start()
    {
        startPosition = transform.position;

        if (visualRenderer == null)
            visualRenderer = GetComponentInChildren<Renderer>();

        if (visualRenderer != null)
            material = visualRenderer.material;
    }

    void Update()
    {
        // Gira o objeto
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Flutua para cima e para baixo
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);

        // Brilho (se tiver material)
        if (material != null && material.HasProperty("_EmissionColor"))
        {
            float emissiveStrength = (Mathf.Sin(Time.time * glowFrequency) + 1f) / 2f;
            Color baseColor = Color.white;
            Color finalColor = baseColor * (glowIntensity * emissiveStrength);
            material.SetColor("_EmissionColor", finalColor);
        }
    }
}
