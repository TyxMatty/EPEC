using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoiceSystemGlobal : MonoBehaviour
{
    [Header("Variaveis Globais de Narrativa")]
    [Header("Base de Dados")]
    [SerializeField] private GlobalStateManager stateManager; 
    
    [Header("Inventario")]
    [SerializeField] private InventoryData inventarioGlobal;
    
    [Header("UI de Escolha")]
    [SerializeField] private GameObject choiceUI; 
    [SerializeField] private TMPro.TextMeshProUGUI[] choiceTexts; 
    [SerializeField] private Color corNormal = Color.white;
    [SerializeField] private Color corSelecionada = Color.yellow; 
    [SerializeField] private TMPro.TextMeshProUGUI textoTempo; 
    
    private bool isChoosing = false;
    private int escolhaAtual = -1; 
    private string variavelAtualParaSalvar = "";
    private bool usarTempoLimite = false; 
    private float tempoRestante = 0f;
    private bool escolhaNeutraAtivada = false;

    // Cache para otimizar a performance
    private DialogueSystemGlobal cachedDialogueSystem;

    void Start()
    {
        choiceUI.SetActive(false);
        cachedDialogueSystem = FindAnyObjectByType<DialogueSystemGlobal>();
    }

    void Update()
    {
        if (!isChoosing) return;
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (usarTempoLimite)
        {
            tempoRestante -= Time.deltaTime;
            textoTempo.text = Mathf.CeilToInt(tempoRestante).ToString();

            if (tempoRestante <= 0)
            {
                FinalizarEscolhaPorTempo();
            }
        }

        if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame)
        {
            MudarEscolha(-1);
        }
        else if (keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame)
        {
            MudarEscolha(1);
        }

        if (keyboard.enterKey.wasPressedThisFrame && escolhaAtual != -1)
        {
            ConfirmarEscolha();
        }
    }

    public void IniciarTelaDeEscolhas(string variavel, string[] opcoes, bool[] valores, bool comTempo, float tempo, bool neutraAtivada)
    {
        variavelAtualParaSalvar = variavel;
        usarTempoLimite = comTempo;
        tempoRestante = tempo;
        escolhaNeutraAtivada = neutraAtivada;
        
        if (usarTempoLimite){
        if(textoTempo != null){
        textoTempo.gameObject.SetActive(true);}
            else
            {
                Debug.Log("Ta faltando o tempo");
            }
        }
        else if(textoTempo != null){
        textoTempo.gameObject.SetActive(false);}

        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < opcoes.Length)
            {
                choiceTexts[i].text = opcoes[i];
                choiceTexts[i].gameObject.SetActive(true);
                choiceTexts[i].color = corNormal; 
            }
            else
            {
                choiceTexts[i].gameObject.SetActive(false);
            }
        }

        escolhaAtual = 0; 
        AtualizarCores();

        choiceUI.SetActive(true);
        isChoosing = true;
    }

    private void MudarEscolha(int direcao)
    {
        int quantidadeDeOpcoesVisiveis = 0;
        foreach (var texto in choiceTexts)
        {
            if (texto.gameObject.activeSelf) quantidadeDeOpcoesVisiveis++;
        }

        escolhaAtual += direcao;
        if (escolhaAtual < 0) escolhaAtual = quantidadeDeOpcoesVisiveis - 1;
        if (escolhaAtual >= quantidadeDeOpcoesVisiveis) escolhaAtual = 0;

        AtualizarCores();
    }

    private void AtualizarCores()
    {
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (choiceTexts[i].gameObject.activeSelf)
            {
                 choiceTexts[i].color = (i == escolhaAtual) ? corSelecionada : corNormal;
            }
        }
    }

    private void ConfirmarEscolha()
    {
        isChoosing = false;
        choiceUI.SetActive(false);
        if(textoTempo != null) textoTempo.gameObject.SetActive(false);

        bool valorDaEscolha = false; 

        if (escolhaAtual == 0) valorDaEscolha = true;
        else if (escolhaAtual == 1) valorDaEscolha = false;
        
        SalvarEscolha(variavelAtualParaSalvar, valorDaEscolha);

        if (cachedDialogueSystem != null) cachedDialogueSystem.podeAvancar = true;
        
        if (cachedDialogueSystem != null) cachedDialogueSystem.EncerrarPosEscolha();
    }

    private void FinalizarEscolhaPorTempo()
    {
         isChoosing = false;
         choiceUI.SetActive(false);
         textoTempo.gameObject.SetActive(false);

         bool valorDaEscolha = escolhaNeutraAtivada; 
         SalvarEscolha(variavelAtualParaSalvar, valorDaEscolha);

         if (cachedDialogueSystem != null) cachedDialogueSystem.podeAvancar = true;

         if (cachedDialogueSystem != null) cachedDialogueSystem.EncerrarPosEscolha();
    }

    public void SalvarEscolha(string nomeVariavel, bool valor)
    {
        if (stateManager == null) 
        {
            Debug.LogError("[ChoiceSystem] StateManager nao esta referenciado no Inspector!");
            return;
        }
        if (string.IsNullOrEmpty(nomeVariavel)) return;

        stateManager.SetVariavel(nomeVariavel, valor);
        Debug.Log($"Variavel '{nomeVariavel}' atualizada no Dicionario para: {valor}");
    }

    public bool ChecarCondicao(string nomeVariavel)
    {
        if (stateManager == null) return false;
        return stateManager.GetVariavel(nomeVariavel);
    }

    public bool ChecarItem(string nomeItem)
    {
        if (inventarioGlobal == null)
        {
            Debug.LogWarning("[ChoiceSystem] Inventario global nulo. Assumindo que nao tem o item.");
            return false;
        }
        if (string.IsNullOrEmpty(nomeItem)) 
        {
            return false;
        }

        bool itemExiste = inventarioGlobal.itens.Exists(item => item.itemName == nomeItem);

        if (!itemExiste)
        {
            Debug.Log($"[ChoiceSystem] O jogador nao tem o item '{nomeItem}'.");
            return false;
        }

        return true;
    }
}
