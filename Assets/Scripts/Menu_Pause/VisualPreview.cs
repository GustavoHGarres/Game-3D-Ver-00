using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPreview : MonoBehaviour
{
    public Transform anchorPreview;        // Vazio onde o visual será instanciado
    public GameObject visualPadrao;        // Prefab padrão do visual

    private GameObject visualAtual;

    void Start()
    {
        TrocarVisual(visualPadrao);        // Começa com o visual padrão
    }

    public void TrocarVisual(GameObject novoVisual)
    {
        // Verifica se está trocando para o mesmo visual
        string nomeNovo = novoVisual.name.Replace("(Clone)", "").Trim();
        string nomeAtual = visualAtual != null ? visualAtual.name.Replace("(Clone)", "").Trim() : "";

        if (visualAtual != null && nomeNovo == nomeAtual)
        {
            Destroy(visualAtual);
            visualAtual = Instantiate(visualPadrao, anchorPreview.position, Quaternion.identity);
        }
        else
        {
            if (visualAtual != null)
                Destroy(visualAtual);

            visualAtual = Instantiate(novoVisual, anchorPreview.position, Quaternion.identity);
        }

        // Organiza hierarquia e transformações
        visualAtual.transform.SetParent(anchorPreview, false);
        visualAtual.transform.localPosition = Vector3.zero;
        visualAtual.transform.localRotation = Quaternion.Euler(0, 180f, 0); // Posição inicial
        visualAtual.transform.localScale = Vector3.one;

        // Aponta o PreviewRotator para o novo visual
        var rotator = anchorPreview.GetComponent<PreviewRotator>();
        if (rotator == null)
            rotator = anchorPreview.gameObject.AddComponent<PreviewRotator>();

        rotator.alvoRotacaoManual = visualAtual.transform; // <- ALTERADO AQUI

        // Remove coliders, rigidbodies etc
        foreach (var col in visualAtual.GetComponentsInChildren<Collider>())
            col.enabled = false;

        foreach (var rb in visualAtual.GetComponentsInChildren<Rigidbody>())
            Destroy(rb);

        foreach (var cc in visualAtual.GetComponentsInChildren<CharacterController>())
            Destroy(cc);

        foreach (var mono in visualAtual.GetComponentsInChildren<MonoBehaviour>())
        {
            if (!(mono is Animator) && !(mono is PreviewRotator))
                Destroy(mono);
        }
    }
}