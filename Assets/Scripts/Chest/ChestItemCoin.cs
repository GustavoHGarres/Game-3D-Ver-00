using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Itens;

public class ChestItemCoin : ChestItemBase
{
     public int coinNumber = 5;
     public GameObject coinObject;

     private List<GameObject> _itens = new List<GameObject>();
     public Vector2 randomRange = new Vector2 (-2f, 2f);

     public float tweenEndTime = 0.5f;

     public override void ShowItem()
     {
         base.ShowItem();
         CreateItens();
     }

     [NaughtyAttributes.Button]
     private void CreateItens()
     {
          for (int i = 0; i < coinNumber; i++)
          {
               var item = Instantiate(coinObject);
               item.transform.position = transform.position + Vector3.forward * Random.Range(randomRange.x, randomRange.y) + Vector3.right * Random.Range(randomRange.x, randomRange.y);
               item.transform.DOScale(0, 1f).SetEase(Ease.OutBack).From();
               _itens.Add(item);
          }
     }

     [NaughtyAttributes.Button]
   public override void Collect()
{
    base.Collect();

    // Acha o player (Instance ou Tag)
    Transform playerT = null;
    if (Player.Instance != null) playerT = Player.Instance.transform;
    else
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) playerT = go.transform;
    }

    foreach (var i in _itens)
    {
        if (i == null) continue;

        // pega o Magnetic (esta no prefab, porem DESABILITADO)
        var mag = i.GetComponent<Magnetic>();
        if (mag == null) mag = i.AddComponent<Magnetic>(); // fallback, se não tiver
        mag.target = playerT;      // opcional (ele tambem acha por Tag/Instance)
        mag.enabled = true;        // << so ativa AGORA, depois que o bau abriu
    }

    _itens.Clear(); // coleta ficara por conta do ItemCollectableBase ao encostar no player
}
}
