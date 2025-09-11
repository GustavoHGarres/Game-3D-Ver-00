using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveMenuUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI playText;  // “Play 2”, “Play 3”…

    void OnEnable()
    {
        // Se o SaveManager ja existir, assina o evento.
        if (SaveManager.Instance != null)
            SaveManager.Instance.FileLoaded += OnFileLoaded;
    }

    void OnDisable()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.FileLoaded -= OnFileLoaded;
    }

    void Start()
    {
        // Opcional: carrega logo ao abrir o menu (para atualizar o PlayX)
        if (SaveManager.Instance != null)
            SaveManager.Instance.Load();
    }

    void OnFileLoaded(SaveSetup s)
    {
        if (playText != null)
            playText.text = "Play" + (s.lastLevel + 1);
    }

    // ----- Chamado pelo botao -----
    public void OnClickLoad()
    {
        SaveManager.Instance.Load();
        // Se quiser ja entrar direto na fase salva:
        // SceneManager.LoadScene(SaveManager.Instance.Setup.lastLevel);
    }

    public void OnClickPlay()
    {
        // Leva o jogador para a proxima fase (ou a fase salva, como preferir)
        int target = SaveManager.Instance.Setup.lastLevel + 1;
        SceneManager.LoadScene(target);
    }

    public void OnClickSave()
    {
        // Se quiser expor no menu (opcional)
        // SaveManager.Instance.Save();  // ou SaveItens()+Save()
        SaveManager.Instance.SaveItens();
    }
}
