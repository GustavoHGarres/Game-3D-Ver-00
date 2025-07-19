using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    public List<string> dialogos;
    private int index = 0;
    private bool isDialogActive = false;

    private void Start()
    {
        dialogPanel.SetActive(false);
    }

    private void Update()
    {
        if (isDialogActive && Input.GetKeyDown(KeyCode.B))
        {
            MostrarProximaFrase();
        }
    }

    public void IniciarDialogo(List<string> frases)
    {
        dialogos = frases;
        index = 0;
        dialogPanel.SetActive(true);
        dialogText.text = dialogos[index];
        isDialogActive = true;
    }

    private void MostrarProximaFrase()
    {
        index++;
        if (index < dialogos.Count)
        {
            dialogText.text = dialogos[index];
        }
        else
        {
            FinalizarDialogo();
        }
    }

    private void FinalizarDialogo()
    {
        dialogPanel.SetActive(false);
        isDialogActive = false;
    }

    public bool DialogoAtivo()
    {
        return isDialogActive;
    }
}