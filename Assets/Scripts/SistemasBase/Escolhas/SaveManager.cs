using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class GameSaveData 
{
    public List<VariavelNarrativa> variaveisSalvas;
    public List<string> itensSalvos;
}

public class SaveManager : MonoBehaviour
{
    [Header("Referencias Globais")]
    public GlobalStateManager stateManager;
    public InventoryData inventory;
    
    private string savePath;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/savegame.json";
        
        if (stateManager != null)
        {
            stateManager.Inicializar(); 
        }
    }

    void Start()
    {
        // Movido para o Start para evitar que outros Awakes leiam dados desatualizados
        CarregarJogo();
    }

        public void SalvarJogo()
    {
        if (stateManager == null || inventory == null) 
        {
            Debug.LogError("[SaveManager] Falha ao salvar: StateManager ou InventoryData estao nulos no Inspector!");
            return;
        }

        GameSaveData data = new GameSaveData();
        data.variaveisSalvas = stateManager.variaveisGlobais;
        data.itensSalvos = new List<string>();

        foreach(var item in inventory.itens)
        {
            if (item != null && !string.IsNullOrEmpty(item.itemID)) 
            {
                data.itensSalvos.Add(item.itemID); 
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Jogo Salvo com Sucesso em: " + savePath);
    }

    public void CarregarJogo()
    {
        if (stateManager == null || inventory == null) return;

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            stateManager.variaveisGlobais = data.variaveisSalvas;
            stateManager.Inicializar(); 

            inventory.itens.Clear();
            
            // Puxamos TODOS os itens possiveis do jogo da pasta Resources
            ItemData[] todosItensDoJogo = Resources.LoadAll<ItemData>("Itens");

            foreach (string savedID in data.itensSalvos)
            {
                // Procuramos na lista o item que tem o ID exato
                ItemData loadedItem = System.Array.Find(todosItensDoJogo, i => i.itemID == savedID);
                
                if (loadedItem != null)
                {
                    inventory.itens.Add(loadedItem);
                }
                else
                {
                    Debug.LogWarning($"[SaveManager] Atencao! Item salvo com ID '{savedID}' nao foi encontrado na pasta Resources/Itens.");
                }
            }
            Debug.Log("Jogo Carregado com Sucesso!");
        }
        else
        {
            Debug.Log("Nenhum save encontrado. Comecando um jogo novo.");
        }
    }

    private void OnApplicationQuit()
    {
        SalvarJogo();
    }
}
