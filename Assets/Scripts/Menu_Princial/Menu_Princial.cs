using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Princial : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("SCN_Fase02"); // troque para o nome da sua cena principal
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo!");
    }

    // Pode adicionar metodos extras para opces no futuro
}