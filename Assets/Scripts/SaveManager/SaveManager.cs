using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private SaveSetup _saveSetup;
    private string _path = Application.streamingAssetsPath + "/save.text";

    public int lastLevel;

    public Action<SaveSetup> FileLoaded;

    public SaveSetup Setup
    {
        get {return _saveSetup;}
    }
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void CreateNewSave()
    {
        _saveSetup = new SaveSetup();
        _saveSetup.lastLevel = 0;
        _saveSetup.checkpointId = 0;
        _saveSetup.playerName = "Gustavo";
    }

    private void Start()
    {
       Invoke(nameof(Load), .1f);
    }

#region 
    [NaughtyAttributes.Button]
    private void Save()
    {
        string setupToJson = JsonUtility.ToJson(_saveSetup, true);
        Debug.Log(setupToJson);
        SaveFile(setupToJson);
    }

    public void SaveItens()
    {
        _saveSetup.coins = Itens.ItemManager.Instance.GetItemByType(Itens.ItemType.COIN).soInt.value;
        _saveSetup.health = Itens.ItemManager.Instance.GetItemByType(Itens.ItemType.LIFE_PACK).soInt.value;
       
       var hb = Player.Instance != null ? Player.Instance.healthBase : null;
       if (hb != null && hb.MaxLife > 0f)
       {
           _saveSetup.health = hb.CurrentLife / hb.MaxLife;   // salva percentual 0..1
       }

       else
       {
           _saveSetup.health = 1f; // fallback: vida cheia
       }

        Save();
    }

    public void SaveName(string text)
    {
         _saveSetup.playerName = text;
         Save();
    }

    public void SaveLastLevel(int level)
    {
         _saveSetup.lastLevel = level;
         SaveItens();
         Save();
    }
    
    public void SaveLastCheckPoint(int checkpointId)
    {
         _saveSetup.checkpointId = checkpointId;
         SaveItens();
         Save();
    }

// Save Vida normalizada
private IEnumerator ApplyHealthWhenPlayerIsReady()
{
    // Espera o Player nascer e ter HealthBase
    yield return new WaitUntil(() => Player.Instance != null && Player.Instance.healthBase != null);

    var hb = Player.Instance.healthBase;

    // Se for save antigo ou valor invalido, considere 1 (vida cheia)
    float pct = (_saveSetup.health <= 0f) ? 1f : Mathf.Clamp01(_saveSetup.health);

    hb.SetLifePercent(pct);   // usa o helper que criamos
}
// ---

// Save roupas
public void SetOutfit(string outfitId)
{
    _saveSetup.outfitId = outfitId;
    Save(); // opcional: se preferir, remova o Save() para não gravar em disco a cada coleta
}

private IEnumerator ApplyWhenPlayerIsReady()
{
    // ---- vida (como você já faz hoje) ----
    yield return new WaitUntil(() => Player.Instance != null && Player.Instance.healthBase != null);

    var hb = Player.Instance.healthBase;
    float pct = (_saveSetup.health <= 0f) ? 1f : Mathf.Clamp01(_saveSetup.health);
    hb.SetLifePercent(pct);

    // ---- roupa (aplica só se for SKIN) ----
    if (!string.IsNullOrEmpty(_saveSetup.outfitId) &&
        System.Enum.TryParse<Cloth.ClothType>(_saveSetup.outfitId, out var ct))
    {
        if (ct == Cloth.ClothType.SKIN && Cloth.ClothManager.Instance != null)
        {
            var setup = Cloth.ClothManager.Instance.GetSetupByType(ct);
            if (setup != null)
            {
                // duração 0 = permanente
                Player.Instance.ChangeTexture(setup, 0f);
            }
        }
        // Se for SPEED/STRONG, deixamos apenas salvo para exibir na UI.
    }
}
// ---

#endregion

    private void SaveFile(string json)
    {
         Debug.Log(_path);
         File.WriteAllText(_path, json);        
    }  

    [NaughtyAttributes.Button]
    public void Load()
    {
        string fileLoaded = "";
        if(File.Exists(_path)) 
        {
            fileLoaded = File.ReadAllText(_path);
            _saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);
            lastLevel = _saveSetup.lastLevel;
        }

        else
        {
            CreateNewSave();
            Save();
        }

        FileLoaded?.Invoke(_saveSetup);

        StartCoroutine(ApplyHealthWhenPlayerIsReady());
    }

    [NaughtyAttributes.Button]
    private void SaveLastOne()
    {
        SaveLastLevel(1);
    }

    [NaughtyAttributes.Button]
    private void SaveLastFive()
    {
        SaveLastLevel(5);
    }

}

[System.Serializable]
public class SaveSetup
{
    public string playerName;
    public int lastLevel;
    public int checkpointId;      // qual checkpoint da fase
    public float health;          // normalizado 0..1
    public string outfitId;      // id da skin/roupa atual
    public float coins;      
}
