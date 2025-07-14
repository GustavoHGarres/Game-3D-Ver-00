using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeclaP_MenuPause : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Armazena o nome da cena atual para retornar depois
            PlayerPrefs.SetString("UltimaCena", SceneManager.GetActiveScene().name);

            // Carrega a cena do menu
            SceneManager.LoadScene("SCN_MenuVisual");
        }
    }
}