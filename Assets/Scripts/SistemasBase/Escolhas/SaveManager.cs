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
        // O local padrao seguro que a Unity escolhe para salvar arquivos em qualquer sistema operacional
        savePath = Application.persistentDataPath + "/gamesave.json";
        
        stateManager.Inicializar(); // cacha o dicionario 
        CarregarJogo();
    }

    public void SalvarJogo()
    {
        GameSaveData data = new GameSaveData();
        data.variaveisSalvas = stateManager.variaveisGlobais;
        data.itensSalvos = new List<string>();
        //futuramente, se tiver, adicionar status aqui.
        // salvamos apenas os nomes dos itens (ScriptableObjects nao salvam no JSON direito)
        foreach(var item in inventory.itens)
        {
            if (item != null) data.itensSalvos.Add(item.name); // Salva o nome do arquivo .asset
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Jogo Salvo com Sucesso em: " + savePath);
    }

    public void CarregarJogo()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            // Carrega variaveis
            stateManager.variaveisGlobais = data.variaveisSalvas;
            stateManager.Inicializar(); // Atualiza o dicionario

            // Carrega o inventario
            inventory.itens.Clear();
            foreach (string itemName in data.itensSalvos)
            {
                // ATENCAO: Isso exige que os seus arquivos de item (Chave.asset) estejam dentro de uma pasta chamada "Resources/Itens"!
                ItemData loadedItem = Resources.Load<ItemData>("Itens/" + itemName);
                if (loadedItem != null)
                {
                    inventory.itens.Add(loadedItem);
                }
            }
            Debug.Log("Jogo Carregado com Sucesso!");
        }
        else
        {
            Debug.Log("Nenhum save encontrado. Comecando um jogo novo.");
        }
    }

    // para voce testar rapidamente, ele salva ao fechar o jogo
    private void OnApplicationQuit()
    {
        SalvarJogo();
        Debug.Log("Jogo salvo");
    }
}