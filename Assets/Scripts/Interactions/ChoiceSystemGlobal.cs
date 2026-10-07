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
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference navigateAction; 
    [SerializeField] private InputActionReference submitAction; 
    
    private bool isChoosing = false;
    private int escolhaAtual = -1; 
    private string variavelAtualParaSalvar = "";
    private bool usarTempoLimite = false; 
    private float tempoRestante = 0f;
    private bool escolhaNeutraAtivada = false;
    private int ultimoSegundoExibido = -1;

    private bool[] valoresDasEscolhas;
    private int totalOpcoesAtuais = 0;
    private System.Action onChoiceMadeCallback;

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

        if (submitAction != null && submitAction.action != null && submitAction.action.WasPressedThisFrame() && escolhaAtual != -1)
        {
            ConfirmarEscolha();
        }
    }

    public void IniciarTelaDeEscolhas(string variavelAlvo, string[] opcoesTextos, bool[] opcoesValores, bool temTempo, float limiteDeTempo, bool escolhaNeutra, System.Action callbackDeSucesso)
    {
        variavelAtualParaSalvar = variavelAlvo;
        isChoosing = true;
        escolhaAtual = 0; 
        totalOpcoesAtuais = opcoesTextos != null ? opcoesTextos.Length : 0;
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

    private void MudarEscolha(int direcao)
    {
        if (totalOpcoesAtuais <= 0) return;

        escolhaAtual += direcao;

        if (escolhaAtual < 0) escolhaAtual = totalOpcoesAtuais - 1;
        else if (escolhaAtual >= totalOpcoesAtuais) escolhaAtual = 0;

        AtualizarUI();
    }

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
        
        onChoiceMadeCallback?.Invoke();
        onChoiceMadeCallback = null;
    }

    private void FinalizarEscolhaPorTempo()
    {
        isChoosing = false;
        if (choiceUI != null) choiceUI.SetActive(false);

        if (!string.IsNullOrEmpty(variavelAtualParaSalvar))
        {
            SalvarEscolha(variavelAtualParaSalvar, escolhaNeutraAtivada);
        }
        
        onChoiceMadeCallback?.Invoke();
        onChoiceMadeCallback = null;
    }

    public void SalvarEscolha(string variavel, bool valor) 
    {
        if (stateManager != null)
        {
            stateManager.SetVariavel(variavel, valor);
            Debug.Log($"[ChoiceSystem] Salvo na Variavel Global '{variavel}' o valor: {valor}");
        }
    }
    
    public bool ChecarCondicao(string variavel)
    {
        if (stateManager != null)
        {
            return stateManager.GetVariavel(variavel);
        }
        return false;
    }
    
    public bool ChecarItem(string itemNameOrID)
    {
        if (inventarioGlobal != null && inventarioGlobal.itens != null)
        {
            foreach(var item in inventarioGlobal.itens)
            {
                if (item != null && (item.itemName == itemNameOrID || item.itemID == itemNameOrID))
                {
                    return true; 
                }
            }
        }
        return false; 
    }
}

