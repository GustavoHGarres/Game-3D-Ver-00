using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoVoltarCenaAnterior : MonoBehaviour
{
    public void Voltar()
    {
        string cenaAnterior = GameManager.Instance.cenaAnterior;
        if (!string.IsNullOrEmpty(cenaAnterior))
        {
            SceneManager.LoadScene(cenaAnterior);
        }
        else
        {
            Debug.LogWarning("Cena anterior não definida. Voltando para SCN_Art_3D por padrão.");
            SceneManager.LoadScene("SCN_Art_3D");
        }
    }
}