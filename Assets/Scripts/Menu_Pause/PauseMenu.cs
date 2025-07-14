using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject painelPause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            bool isActive = painelPause.activeSelf;
            painelPause.SetActive(!isActive);

            // Pausa o jogo quando o menu estiver ativo
            Time.timeScale = isActive ? 1f : 0f;
        }
    }
}