using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Origem: Assets/Scripts/Interactions/ChoiceSystemGlobal.cs
// Painel de escolhas: Navigate troca a opcao, Submit confirma e o bool da opcao e gravado no GlobalStateManager
public class ChoiceSystemGlobal : MonoBehaviour
{
    [Header("Variaveis Globais de Narrativa")]
    [Header("Base de Dados")]
    // Asset onde as escolhas sao gravadas como flags
    [SerializeField] private GlobalStateManager stateManager; 
    
    [Header("Inventario")]
    // Mochila consultada por ChecarItem (condicoes de dialogo)
    [SerializeField] private InventoryData inventarioGlobal;
    
    [Header("UI de Escolha")]
    // Painel da Canvas com as opcoes
    [SerializeField] private GameObject choiceUI; 
    // Textos das opcoes, um por botao, na ordem do JSON
    [SerializeField] private TMPro.TextMeshProUGUI[] choiceTexts; 
    // Cor da opcao normal e da selecionada
    [SerializeField] private Color corNormal = Color.white;
    [SerializeField] private Color corSelecionada = Color.yellow; 
    // Contador regressivo (opcional; nulo na cena 0)
    [SerializeField] private TMPro.TextMeshProUGUI textoTempo; 
    
    [Header("Input Actions")]
    // Navigate (W/S, setas) troca a opcao; Submit (Enter) confirma
    [SerializeField] private InputActionReference navigateAction; 
    [SerializeField] private InputActionReference submitAction; 
    
    // Estado da escolha em andamento
    private bool isChoosing = false;
    private int escolhaAtual = -1; 
    private string variavelAtualParaSalvar = "";
    private bool usarTempoLimite = false; 
    private float tempoRestante = 0f;
    private bool escolhaNeutraAtivada = false;
    private int ultimoSegundoExibido = -1;
    // Frame em que a tela abriu (o Submit desse frame ainda e o Enter da ultima fala)
    private int frameAbertura = -1;

    private bool[] valoresDasEscolhas;
    private int totalOpcoesAtuais = 0;
    // Recebe o indice da opcao escolhida (-1 quando o tempo acabou)
    private System.Action<int> onChoiceMadeCallback;

    // Assets usados pelo DialogueSystemGlobal para avaliar Condicoes e aplicar Efeitos
    public GlobalStateManager Estado => stateManager;
    public InventoryData Inventario => inventarioGlobal;

    void OnEnable()
    {
        if (navigateAction != null && navigateAction.action != null)
        {
            navigateAction.action.Enable();
        }
        if (submitAction != null && submitAction.action != null)
        {
            submitAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (navigateAction != null && navigateAction.action != null)
        {
            navigateAction.action.Disable();
        }
        if (submitAction != null && submitAction.action != null)
        {
            submitAction.action.Disable();
        }
    }

    void Start()
    {
        if (choiceUI != null) choiceUI.SetActive(false);
    }

    void Update()
    {
        if (!isChoosing) return;

        if (usarTempoLimite)
        {
            tempoRestante -= Time.deltaTime;
            int segundoAtual = Mathf.CeilToInt(tempoRestante);
            if (segundoAtual != ultimoSegundoExibido)
            {
                ultimoSegundoExibido = segundoAtual;
                if (textoTempo != null) textoTempo.text = segundoAtual.ToString();
            }

            if (tempoRestante <= 0)
            {
                FinalizarEscolhaPorTempo();
            }
        }

        if (navigateAction != null && navigateAction.action != null && navigateAction.action.WasPressedThisFrame())
        {
            Vector2 nav = navigateAction.action.ReadValue<Vector2>();
            if (nav.y > 0.5f) MudarEscolha(-1);
            else if (nav.y < -0.5f) MudarEscolha(1);
        }

        // Submit e o mesmo action do DialogueSystemGlobal: ignora o frame de abertura para o Enter da ultima fala nao confirmar a opcao 0
        if (submitAction != null && submitAction.action != null && submitAction.action.WasPressedThisFrame() && escolhaAtual != -1 && Time.frameCount != frameAbertura)
        {
            ConfirmarEscolha();
        }
    }

    // Abre o painel com as opcoes do JSON; callbackDeSucesso recebe o indice escolhido depois de gravar a escolha
    public void IniciarTelaDeEscolhas(string variavelAlvo, string[] opcoesTextos, bool[] opcoesValores, bool temTempo, float limiteDeTempo, bool escolhaNeutra, System.Action<int> callbackDeSucesso)
    {
        variavelAtualParaSalvar = variavelAlvo;
        isChoosing = true;
        frameAbertura = Time.frameCount;
        escolhaAtual = 0;
        totalOpcoesAtuais = opcoesTextos != null ? opcoesTextos.Length : 0;
        int botoes = choiceTexts != null ? choiceTexts.Length : 0;
        if (totalOpcoesAtuais > botoes)
        {
            // Sem isso daria para selecionar uma opcao que nao aparece na tela
            Debug.LogWarning($"[ChoiceSystem] {totalOpcoesAtuais} opcoes mas so {botoes} textos na UI; as extras foram cortadas.");
            totalOpcoesAtuais = botoes;
        }
        usarTempoLimite = temTempo;
        tempoRestante = limiteDeTempo;
        escolhaNeutraAtivada = escolhaNeutra;
        onChoiceMadeCallback = callbackDeSucesso;
        valoresDasEscolhas = opcoesValores;

        if (choiceUI != null) choiceUI.SetActive(true);
        if (textoTempo != null) textoTempo.gameObject.SetActive(temTempo);

        if (choiceTexts != null)
        {
            for (int i = 0; i < choiceTexts.Length; i++)
            {
                if (choiceTexts[i] == null) continue;

                if (opcoesTextos != null && i < opcoesTextos.Length)
                {
                    choiceTexts[i].gameObject.SetActive(true);
                    choiceTexts[i].text = opcoesTextos[i];
                }
                else
                {
                    choiceTexts[i].gameObject.SetActive(false);
                }
            }
        }

        AtualizarUI();
    }

    // Move a selecao com wrap-around
    private void MudarEscolha(int direcao)
    {
        if (totalOpcoesAtuais <= 0) return;

        escolhaAtual += direcao;

        if (escolhaAtual < 0) escolhaAtual = totalOpcoesAtuais - 1;
        else if (escolhaAtual >= totalOpcoesAtuais) escolhaAtual = 0;

        AtualizarUI();
    }

    // Pinta a opcao selecionada
    private void AtualizarUI()
    {
        if (choiceTexts != null)
        {
            for (int i = 0; i < choiceTexts.Length; i++)
            {
                if (choiceTexts[i] != null && choiceTexts[i].gameObject.activeSelf)
                {
                    choiceTexts[i].color = (i == escolhaAtual) ? corSelecionada : corNormal;
                }
            }
        }
    }

    // Grava o bool da opcao selecionada (true se faltar valor) e avisa o dialogo
    private void ConfirmarEscolha()
    {
        isChoosing = false;
        if (choiceUI != null) choiceUI.SetActive(false);

        if (!string.IsNullOrEmpty(variavelAtualParaSalvar))
        {
            bool valorEscolhido = true; 
            if (valoresDasEscolhas != null && escolhaAtual >= 0 && escolhaAtual < valoresDasEscolhas.Length)
            {
                valorEscolhido = valoresDasEscolhas[escolhaAtual];
            }
            SalvarEscolha(variavelAtualParaSalvar, valorEscolhido);
        }

        var callback = onChoiceMadeCallback;
        onChoiceMadeCallback = null;
        callback?.Invoke(escolhaAtual);
    }

    // Timer zerou: grava escolhaNeutra na mesma flag
    // REVIEW: nao responder vira um Sim/Nao comum e a Rota N (hesitar) nao da para detectar depois.
    // Considerar gravar tambem uma flag propria (ex: variavel + "_semResposta").
    private void FinalizarEscolhaPorTempo()
    {
        isChoosing = false;
        if (choiceUI != null) choiceUI.SetActive(false);

        if (!string.IsNullOrEmpty(variavelAtualParaSalvar))
        {
            SalvarEscolha(variavelAtualParaSalvar, escolhaNeutraAtivada);
        }

        var callback = onChoiceMadeCallback;
        onChoiceMadeCallback = null;
        callback?.Invoke(-1);
    }

    // Grava uma flag no GlobalStateManager (usado tambem pelo DialogueSystemGlobal)
    public void SalvarEscolha(string variavel, bool valor) 
    {
        if (stateManager != null)
        {
            stateManager.SetVariavel(variavel, valor);
            Debug.Log($"[ChoiceSystem] Salvo na Variavel Global '{variavel}' o valor: {valor}");
        }
    }
    
    // Le uma flag do GlobalStateManager (false se nao existir)
    public bool ChecarCondicao(string variavel)
    {
        if (stateManager != null)
        {
            return stateManager.GetVariavel(variavel);
        }
        return false;
    }
    
    // true se a mochila tem um item com esse itemName ou itemID
    public bool ChecarItem(string itemNameOrID)
    {
        return inventarioGlobal != null && inventarioGlobal.ContemItem(itemNameOrID);
    }
}

