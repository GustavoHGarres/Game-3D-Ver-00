using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndGameCheckpoint : MonoBehaviour
{
    public List<GameObject> endGameObjects;
    private bool _endGame = false;

    public int currentLevel = 1;
    public int checkpointId = 1;

    public void Awake()
    {
        endGameObjects.ForEach(i => i.SetActive(false));
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.transform.GetComponent<Player>();

        if (! _endGame && p != null)
        {
            ShowEndGame();
        }
    }

    private void ShowEndGame()
    {
    if (_endGame) return;
    _endGame = true;

    foreach (var i in endGameObjects)
    {
        i.SetActive(true);
        i.transform.DOScale(0, .2f).SetEase(Ease.OutBack).From();
    }

    // Se este é um totem de checkpoint:
    SaveManager.Instance.SaveLastCheckPoint(checkpointId);

    // Se este é um totem de fim de fase (Victory), aí sim:
    // SaveManager.Instance.SaveLastLevel(currentLevel);
   }
}
