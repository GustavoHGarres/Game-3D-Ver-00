using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewRotator : MonoBehaviour
{
    public Transform alvoRotacaoManual; // Arraste aqui só se quiser forçar
    public float velocidade = 30f;

    private Transform alvoDetectado;

    void Start()
    {
        if (alvoRotacaoManual != null)
        {
            alvoDetectado = alvoRotacaoManual;
            Debug.Log("PreviewRotator: Usando alvo manual " + alvoDetectado.name);
        }
        else
        {
            // Busca automática
            SkinnedMeshRenderer visual = GetComponentInChildren<SkinnedMeshRenderer>();
            if (visual != null)
            {
                alvoDetectado = visual.transform;
                Debug.Log("PreviewRotator: Alvo detectado automaticamente: " + alvoDetectado.name);
            }
            else
            {
                Debug.LogWarning("PreviewRotator: Nenhum SkinnedMeshRenderer encontrado.");
            }
        }
    }

    void Update()
    {
        if (alvoDetectado != null)
        {
            alvoDetectado.Rotate(Vector3.up * velocidade * Time.deltaTime, Space.World);
            Debug.Log("Girando: " + alvoDetectado.name);
        }
    }
}