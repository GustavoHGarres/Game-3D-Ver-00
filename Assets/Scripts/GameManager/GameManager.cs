using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using Ebac.StateMachine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameStates
    {
        INTRO,
        GAMEPLAY,
        PAUSE,
        WIN,
        LOSE
    }

    public StateMachine<GameStates> stateMachine;

    [Header("Progressão de Fase")]
    public GameObject geleiaPortal;
    private int inimigosRestantes = 0;

    [Header("Aparição da Geleia")]
    public float tempoParaAparecerGeleia = 5f;

    [Header("Recompensas de Visual")]
    public List<GameObject> caixasDeRecompensa;
    private GameObject prefabSelecionado;
    private GameObject _visualSelecionado;

    [Header("Visual do Jogador")]
    public GameObject defaultPrefab;

   
    private void Start()
    {
        Init();
        
        //Interacao quando a geleia aparece quando os inimigos morrem
        if (geleiaPortal != null)
           {
                geleiaPortal.SetActive(false);
           }
        //Interacao quando a geleia aparece quando os inimigos morrem   

    }

    public void Init()
    {
        stateMachine = new StateMachine<GameStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(GameStates.INTRO, new StateBase());
        stateMachine.RegisterStates(GameStates.GAMEPLAY, new StateBase());
        stateMachine.RegisterStates(GameStates.PAUSE, new StateBase());
        stateMachine.RegisterStates(GameStates.WIN, new StateBase());
        stateMachine.RegisterStates(GameStates.LOSE, new StateBase());

        stateMachine.SwitchState(GameStates.INTRO);
    }

    public void RegistrarInimigo()
    {
        inimigosRestantes++;
    }

#region Integrando transicao de cenas

    public void InimigoMorto()
    {
        inimigosRestantes--;
        Debug.Log($"Inimigo derrotado! Restantes: {inimigosRestantes}"); //Testa se tem inimgos com o script EnemyBase

        if (inimigosRestantes <= 0 && geleiaPortal != null)
        {
            Debug.Log("Todos os inimigos morreram. Geleia ativada!"); //Testa se tem inimgos com o script EnemyBase
            StartCoroutine(AguardarParaAtivarGeleia()); // Integra um delay de 5 segundos para o geleia aparecer/ 
        }
    }

// Integra um delay de 5 segundos para o geleia aparecer
    private IEnumerator AguardarParaAtivarGeleia()
    {
         yield return new WaitForSeconds(tempoParaAparecerGeleia);
         geleiaPortal.SetActive(true);
         FadeInMaterial(geleiaPortal); //Surge a geleia suavimente
         Debug.Log("Geleia ativada!");

         // Integracao da recompensa dentro da caixa

         geleiaPortal.SetActive(true);
         FadeInMaterial(geleiaPortal);

         foreach (var caixa in caixasDeRecompensa)
               {
                   if (caixa != null)
                      {
                            caixa.SetActive(true); // Ativa as caixas
                            FadeInMaterial(caixa); // Aparece suavemente
                      }
}

         // Integracao da recompensa dentro da caixa

    }
// Integra um delay de 5 segundos para o geleia aparecer

    public void IrParaProximaCena()
    {
        int cenaAtual = SceneManager.GetActiveScene().buildIndex;
        int proximaCena = cenaAtual + 1;

        if (proximaCena < SceneManager.sceneCountInBuildSettings)
           {
               SceneManager.LoadScene(proximaCena);
           }
        else
           {
               Debug.Log("Não há mais cenas para carregar.");
               // Aqui você pode chamar uma tela de créditos ou voltar pro menu
           }
    }

#endregion    

#region Incrementa uma suavidade quando o geleia aparece

        private void FadeInMaterial(GameObject alvo)
        {
             Renderer renderer = alvo.GetComponentInChildren<Renderer>();

             if (renderer != null)
             {
                 Material mat = renderer.material;
                 mat.shader = Shader.Find("Standard");
                 mat.SetFloat("_Mode", 2); // modo Fade
                 mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                 mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                 mat.SetInt("_ZWrite", 0);
                 mat.DisableKeyword("_ALPHATEST_ON");
                 mat.EnableKeyword("_ALPHABLEND_ON");
                 mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                 mat.renderQueue = 3000;

                 StartCoroutine(FadeInAlpha(mat));
            }
        }

        private IEnumerator FadeInAlpha(Material mat)
        {
              Color cor = mat.color;
              float tempo = 0f;
              float duracao = 1.5f;

              cor.a = 0f;
              mat.color = cor;

              while (tempo < duracao)
                    {
                         cor.a = Mathf.Lerp(0f, 1f, tempo / duracao);
                         mat.color = cor;
                         tempo += Time.deltaTime;
                          yield return null;
                    }

                         cor.a = 1f;
                         mat.color = cor;
        }
        
#endregion   

#region Integrando a recompensas dentro da caixa

         public void SelecionarVisual(GameObject novoPrefab)
         {
               prefabSelecionado = novoPrefab;
               Debug.Log("Visual selecionado: " + novoPrefab.name);

               // Salvar nome no PlayerPrefs
               PlayerPrefs.SetString("VISUAL_SELECIONADO", novoPrefab.name);
               PlayerPrefs.Save();

               foreach (var caixa in caixasDeRecompensa)
                     {
                           if (caixa != null && caixa != novoPrefab)
                            {
                                 caixa.SetActive(false);
                            }
                     }
            }

        // Use isso na proxima fase para aplicar o visual escolhido
        public GameObject GetVisualSelecionado()
        {
               return prefabSelecionado != null ? prefabSelecionado : defaultPrefab;
        }

#endregion

#region Aplica o novo visual na nova cena

public void AplicarVisualNoAstronauta(GameObject player)
{
    string nomeVisual = PlayerPrefs.GetString("VISUAL_SELECIONADO", "");

    if (!string.IsNullOrEmpty(nomeVisual))
    {
        GameObject visualPrefab = Resources.Load<GameObject>(nomeVisual);
        if (visualPrefab != null)
        {
            GameObject novoVisual = Instantiate(visualPrefab, player.transform.position, player.transform.rotation, player.transform);
            
            // Desativa o visual antigo (opcional)
            foreach (Transform child in player.transform)
            {
                if (child.gameObject != novoVisual)
                    child.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"Visual '{nomeVisual}' não encontrado em Resources!");
        }
    }
}

#endregion

    public void InitGame()
    {
        // Código de início do jogo, se quiser usar
    }

}