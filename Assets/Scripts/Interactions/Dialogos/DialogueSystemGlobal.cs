using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

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
    [SerializeField] private ChoiceSystemGlobal choiceSystem; 

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction; // enter, click, botao A
    
    private string[] dialogueLines;
    private int letraAtual = 0; 
    private bool isTyping = false; 
    private bool isTransitioning = false; 
    public bool podeAvancar = false; 
    
    private DialogueData interacaoAtual; 
    private bool usouFalasAlternativas = false;
    private System.Action onDialogueEndedCallback;

    void OnEnable()
    {
        submitAction?.action.Enable();
    }

    void OnDisable()
    {
        submitAction?.action.Disable();
    }

    void Awake()
    {
        if (nextDialogueBox != null)
        {
            nextDialogueBox.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(isTransitioning || !podeAvancar) return;
        
        if (submitAction != null && submitAction.action.WasPressedThisFrame())
        {
            if (isTyping)
            {
                StopAllCoroutines();
                if (dialogueText != null) dialogueText.text = dialogueLines[letraAtual];
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
            StartCoroutine(DigitarLinha());
        }
        StartCoroutine(EsperaEHabitilitaAvanco()); 
    }

    private IEnumerator EsperaEHabitilitaAvanco()
    {
        yield return new WaitForSeconds(0.2f);
        podeAvancar = true;
    }

    private void ProximoDialogo()
    {
        letraAtual++;
        if (dialogueLines != null && letraAtual < dialogueLines.Length)
        {
            StartCoroutine(DigitarLinha());
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
            if (usouFalasAlternativas && interacaoAtual.dialogoAlternativoAlteraVariavel)
            {
                choiceSystem.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogoAlternativo, true);
            }
            else if (!usouFalasAlternativas && interacaoAtual.alteraVariavel)
            {
                choiceSystem.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogo, true); 
            }
        }

        onDialogueEndedCallback?.Invoke();
        onDialogueEndedCallback = null;

        gameObject.SetActive(false); 
    }

    public void IniciarDialogo(DialogueData data, System.Action onComplete = null) 
    {
        interacaoAtual = data; 
        onDialogueEndedCallback = onComplete;
        
        if (interacaoAtual.temFrasesAlternativas && choiceSystem != null)
        {
            bool expectedValue = !interacaoAtual.esperaCondicaoFalsa;
            bool condicaoSatisfeita = false;

            if (!string.IsNullOrEmpty(interacaoAtual.variavelDaCondicao))
            {
                bool result = choiceSystem.ChecarCondicao(interacaoAtual.variavelDaCondicao);
                if (result == expectedValue) condicaoSatisfeita = true;
            }
            
            if (!string.IsNullOrEmpty(interacaoAtual.itemDaCondicao))
            {
                bool result = choiceSystem.ChecarItem(interacaoAtual.itemDaCondicao);
                if (result == expectedValue) condicaoSatisfeita = true;
            }

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

    public void EncerrarPosEscolha() 
    {
       StartCoroutine(Transicao());
    }
}

