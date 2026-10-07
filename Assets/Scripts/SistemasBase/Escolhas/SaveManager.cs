using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class GameSaveData 
{
    // Lista de flags booleanas da narrativa salvas
    public List<VariavelNarrativa> variaveisSalvas;
    // Lista de afinidades/reputacoes com NPCs e animais salvas
    public List<Reputacao> reputacoesSalvas;
    // Lista de IDs dos itens que estao no inventario do jogador
    public List<string> itensSalvos;
}

// Origem: Assets/Scripts/SistemasBase/Escolhas/SaveManager.cs
// Componente responsavel por salvar e carregar o estado narrativo e inventario no disco
public class SaveManager : MonoBehaviour
{
    [Header("Referencias Globais")]
    // Referencia ao ScriptableObject GlobalStateManager que armazena as variaveis e reputacao
    public GlobalStateManager stateManager;
    // Referencia ao ScriptableObject InventoryData que contem os itens do Player
    public InventoryData inventory;
    
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        
        if (stateManager != null)
        {
            stateManager.Inicializar(); 
        }
    }

    void Start()
    {
        CarregarJogo();
    }

    // Metodo publico para gravar o progresso atual do jogador em arquivo JSON
    public void SalvarJogo()
    {
        if (stateManager == null || inventory == null) 
        {
            Debug.LogError("[SaveManager] Falha ao salvar: StateManager ou InventoryData estao nulos no Inspector!");
            return;
        }

        try
        {
            GameSaveData data = new GameSaveData();
            data.variaveisSalvas = stateManager.variaveisGlobais != null ? new List<VariavelNarrativa>(stateManager.variaveisGlobais) : new List<VariavelNarrativa>();
            data.reputacoesSalvas = stateManager.reputacoes != null ? new List<Reputacao>(stateManager.reputacoes) : new List<Reputacao>();
            data.itensSalvos = new List<string>();

            if (inventory.itens != null)
            {
                foreach (var item in inventory.itens)
                {
                    if (item != null && !string.IsNullOrEmpty(item.itemID)) 
                    {
                        data.itensSalvos.Add(item.itemID); 
                    }
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log("[SaveManager] Jogo salvo com sucesso em: " + savePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SaveManager] Erro ao gravar arquivo de save: " + ex.Message);
        }
    }

    // Metodo publico para carregar o arquivo JSON e restaurar o estado do jogo
    public void CarregarJogo()
    {
        if (stateManager == null || inventory == null) return;

        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

                if (data != null)
                {
                    if (data.variaveisSalvas != null)
                    {
                        stateManager.variaveisGlobais = data.variaveisSalvas;
                    }
                    if (data.reputacoesSalvas != null)
                    {
                        stateManager.reputacoes = data.reputacoesSalvas;
                    }
                    stateManager.Inicializar(); 

                    if (inventory.itens != null)
                    {
                        inventory.itens.Clear();
                    }
                    else
                    {
                        inventory.itens = new List<ItemData>();
                    }
                    
                    if (data.itensSalvos != null && data.itensSalvos.Count > 0)
                    {
                        ItemData[] todosItensDoJogo = Resources.LoadAll<ItemData>("Itens");

                        foreach (string savedID in data.itensSalvos)
                        {
                            ItemData loadedItem = System.Array.Find(todosItensDoJogo, i => i != null && i.itemID == savedID);
                            
                            if (loadedItem != null)
                            {
                                inventory.itens.Add(loadedItem);
                            }
                            else
                            {
                                Debug.LogWarning($"[SaveManager] Atencao! Item salvo com ID '{savedID}' nao foi encontrado na pasta Resources/Itens.");
                            }
                        }
                    }
                    Debug.Log("[SaveManager] Jogo carregado com sucesso!");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[SaveManager] Erro ao ler arquivo de save: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("[SaveManager] Nenhum save encontrado. Comecando um jogo novo.");
        }
    }

    private void OnApplicationQuit()
    {
        SalvarJogo();
    }
}
