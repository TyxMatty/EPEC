using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

// Sistema central de exibicao de dialogos, animacao typewriter de texto e gatilho de escolhas
public class DialogueSystemGlobal : MonoBehaviour
{
    [Header("Configuracoes do Sistema de Dialogo")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private Transform nextDialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [Header("Efeitos de Transicao")] 
    [SerializeField] private CanvasGroup canvasGroup; 
    [SerializeField] private float fadeDuration = 0.5f; 
    
    [Header("Integracao de Escolhas")]
    // Referencia ao sistema global de escolhas para iniciar telas de selecao
    [SerializeField] private ChoiceSystemGlobal choiceSystem; 

    [Header("Input Actions")]
    // Acao de input configurada para avancar ou acelerar falas (Enter, Botao A, Click)
    [SerializeField] private InputActionReference submitAction; 
    
    private string[] dialogueLines;
    private int letraAtual = 0; 
    private bool isTyping = false; 
    private bool isTransitioning = false; 

    // Variavel publica que sinaliza se o jogador ja pode pressionar o botao para avancar a fala
    public bool podeAvancar = false; 
    
    private DialogueData interacaoAtual; 
    private bool usouFalasAlternativas = false;
    private System.Action onDialogueEndedCallback;
    private Coroutine typingCoroutine;

    void OnEnable()
    {
        if (submitAction != null && submitAction.action != null)
        {
            submitAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (submitAction != null && submitAction.action != null)
        {
            submitAction.action.Disable();
        }

        // Garante que se o objeto for desativado inesperadamente, quem estiver aguardando seja liberado
        if (onDialogueEndedCallback != null)
        {
            onDialogueEndedCallback.Invoke();
            onDialogueEndedCallback = null;
        }
    }

    void Awake()
    {
        if (nextDialogueBox != null)
        {
            nextDialogueBox.gameObject.SetActive(false);
        }
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    void Update()
    {
        if (isTransitioning || !podeAvancar) return;
        
        if (submitAction != null && submitAction.action != null && submitAction.action.WasPressedThisFrame())
        {
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                if (dialogueText != null && dialogueLines != null && letraAtual < dialogueLines.Length)
                {
                    dialogueText.text = dialogueLines[letraAtual];
                }
                isTyping = false;
            }
            else
            {
                ProximoDialogo();   
            }
        }
    }

    private void StartDialogue()
    {
        letraAtual = 0;
        if (dialogueLines != null && dialogueLines.Length > 0)
        {
            typingCoroutine = StartCoroutine(DigitarLinha());
        }
        StartCoroutine(EsperaEHabilitaAvanco()); 
    }

    private IEnumerator EsperaEHabilitaAvanco()
    {
        yield return new WaitForSeconds(0.2f);
        podeAvancar = true;
    }

    private void ProximoDialogo()
    {
        letraAtual++;
        if (dialogueLines != null && letraAtual < dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(DigitarLinha());
        } 
        else
        {
            if (interacaoAtual != null && interacaoAtual.terminaEmEscolha && choiceSystem != null)
            {
                Debug.Log("[DialogueSystem] Dialogo terminou em escolha. Iniciando escolhas...");
                podeAvancar = false; // Trava o jogador na tela de escolha
                choiceSystem.IniciarTelaDeEscolhas( 
                    interacaoAtual.variavelParaSalvar, 
                    interacaoAtual.textosDasOpcoes != null ? interacaoAtual.textosDasOpcoes.ToArray() : new string[0],
                    interacaoAtual.valoresDasOpcoes != null ? interacaoAtual.valoresDasOpcoes.ToArray() : new bool[0],
                    interacaoAtual.temTempoLimite,
                    interacaoAtual.tempoLimite,
                    interacaoAtual.escolhaNeutra,
                    () => {
                        podeAvancar = true;
                        EncerrarPosEscolha();
                    }
                );
            }
            else
            {
                StartCoroutine(Transicao());
            }
        }
    }

    private IEnumerator DigitarLinha()
    {
        isTyping = true;
        if (dialogueText != null) dialogueText.text = "";
        
        if (dialogueLines != null && letraAtual < dialogueLines.Length)
        {
            foreach (char letter in dialogueLines[letraAtual].ToCharArray())
            {
                if (dialogueText != null) dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isTyping = false;
    }

    private IEnumerator Transicao()
    {
        isTransitioning = true;
        if (canvasGroup != null)
        {
            float tempo = 0f;
            while (tempo < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, tempo / fadeDuration);
                tempo += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }
        
        if (nextDialogueBox != null)
        {
            nextDialogueBox.gameObject.SetActive(true);
        }
        
        if (interacaoAtual != null && choiceSystem != null) 
        {
            if (usouFalasAlternativas)
            {
                if (interacaoAtual.dialogoAlternativoAlteraVariavel)
                    choiceSystem.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogoAlternativo, true);
                
                if (interacaoAtual.dialogoAlternativoAlteraReputacao && ReputationSystem.Instance != null)
                    ReputationSystem.Instance.AdicionarReputacao(interacaoAtual.alvoReputacaoAlternativo, interacaoAtual.pontosReputacaoAlternativo);
            }
            else
            {
                if (interacaoAtual.alteraVariavel)
                    choiceSystem.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogo, true); 
                
                if (interacaoAtual.alteraReputacao && ReputationSystem.Instance != null)
                    ReputationSystem.Instance.AdicionarReputacao(interacaoAtual.alvoReputacao, interacaoAtual.pontosReputacao);
            }
        }

        onDialogueEndedCallback?.Invoke();
        onDialogueEndedCallback = null;

        gameObject.SetActive(false); 
    }

    public void IniciarDialogo(DialogueData data, System.Action onComplete = null) 
    {
        if (data == null)
        {
            Debug.LogError("[DialogueSystem] DialogueData recebido e nulo!");
            onComplete?.Invoke();
            return;
        }

        interacaoAtual = data; 
        onDialogueEndedCallback = onComplete;
        if (dialogueText != null) dialogueText.text = "";
        
        if (interacaoAtual.temFrasesAlternativas && choiceSystem != null)
        {
            bool expectedValue = !interacaoAtual.esperaCondicaoFalsa;
            bool condicaoVariavelValida = true;
            bool condicaoItemValida = true;

            if (!string.IsNullOrEmpty(interacaoAtual.variavelDaCondicao))
            {
                bool result = choiceSystem.ChecarCondicao(interacaoAtual.variavelDaCondicao);
                condicaoVariavelValida = (result == expectedValue);
            }
            
            if (!string.IsNullOrEmpty(interacaoAtual.itemDaCondicao))
            {
                bool result = choiceSystem.ChecarItem(interacaoAtual.itemDaCondicao);
                condicaoItemValida = (result == expectedValue);
            }

            bool condicaoSatisfeita = condicaoVariavelValida && condicaoItemValida;

            if (condicaoSatisfeita)
            {
                dialogueLines = interacaoAtual.falasAlternativas != null ? interacaoAtual.falasAlternativas.ToArray() : new string[0]; 
                usouFalasAlternativas = true;
            }
            else
            {
                dialogueLines = interacaoAtual.falas != null ? interacaoAtual.falas.ToArray() : new string[0]; 
                usouFalasAlternativas = false;
            }
        }
        else
        {
            dialogueLines = interacaoAtual.falas != null ? interacaoAtual.falas.ToArray() : new string[0]; 
            usouFalasAlternativas = false;
        }

        isTransitioning = false;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; 
        }
        StartDialogue();
    }

    // Metodo publico chamado pelo sistema de escolhas ao finalizar uma opcao
    public void EncerrarPosEscolha() 
    {
       StartCoroutine(Transicao());
    }
}



