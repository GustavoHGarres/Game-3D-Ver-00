using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogoController : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] falas; // Conversa completa
    public GameObject painelDialogo;
    public TextMeshProUGUI textoDialogo;

    private int indiceFala = 0;
    private bool estaProximo = false;
    private bool dialogoAtivo = false;

    void Update()
    {
        if (estaProximo)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (!dialogoAtivo)
                {
                    IniciarDialogo();
                }
                else
                {
                    ProximaFala();
                }
            }

            // Aqui você já tem a lógica de "segurar B" para mudar de fase
        }
    }

    void IniciarDialogo()
    {
        indiceFala = 0;
        dialogoAtivo = true;
        painelDialogo.SetActive(true);
        textoDialogo.text = falas[indiceFala];
    }

    void ProximaFala()
    {
        indiceFala++;

        if (indiceFala < falas.Length)
        {
            textoDialogo.text = falas[indiceFala];
        }
        else
        {
            FinalizarDialogo();
        }
    }

    void FinalizarDialogo()
    {
        dialogoAtivo = false;
        painelDialogo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaProximo = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaProximo = false;
            Debug.Log("Entrou no trigger com: " + other.name);
        }
    }
}