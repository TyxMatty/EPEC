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
    [SerializeField] private ChoiceSystemGlobal GameManager; 
    
    private string[] dialogueLines;
    private int letraAtual = 0; 
    private bool isTyping = false; 
    private bool isTransitioning = false; 
    public bool podeAvancar = false; 
    
    private DialogueData interacaoAtual; 
    private bool usouFalasAlternativas = false;

    void Start()
    {
        if (nextDialogueBox != null)
        {
            nextDialogueBox.gameObject.SetActive(false);
        }
        gameObject.SetActive(false); 
    }

    void Update()
    {
        if(isTransitioning || !podeAvancar) return;
        var keyboard = Keyboard.current;
        if(keyboard == null) return;
        
        if (keyboard.enterKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[letraAtual];
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
        if (letraAtual < dialogueLines.Length)
        {
            StartCoroutine(DigitarLinha());
        } 
        else
        {
            if (interacaoAtual != null && interacaoAtual.terminaEmEscolha && GameManager != null)
            {
                Debug.Log("[DialogueSystem] Dialogo terminou em escolha. Iniciando...");
                GameManager.IniciarTelaDeEscolhas( 
                    interacaoAtual.variavelParaSalvar, 
                    interacaoAtual.textosDasOpcoes.ToArray(),
                    interacaoAtual.valoresDasOpcoes.ToArray(),
                    interacaoAtual.temTempoLimite,
                    interacaoAtual.tempoLimite,
                    interacaoAtual.escolhaNeutra
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
        dialogueText.text = "";
        foreach (char letter in dialogueLines[letraAtual].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
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
        gameObject.SetActive(false); 
        
        MovementScript movementScript = FindAnyObjectByType<MovementScript>();
        if (movementScript != null)
        {
            movementScript.podeMover = true; 
        }
        
        if (interacaoAtual != null && GameManager != null) 
        {
            if (usouFalasAlternativas && interacaoAtual.dialogoAlternativoAlteraVariavel)
            {
                GameManager.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogoAlternativo, true);
                Debug.Log($"[DialogueSystem] Variavel global {interacaoAtual.variavelAlteradaPorDialogoAlternativo} alterada para true (Falas Alternativas).");
            }
            else if (!usouFalasAlternativas && interacaoAtual.alteraVariavel)
            {
                GameManager.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogo, true); 
                Debug.Log($"[DialogueSystem] Variavel global {interacaoAtual.variavelAlteradaPorDialogo} alterada para true (Falas Normais).");
            }
        }
    }

    public void IniciarDialogo(DialogueData data) 
    {
        interacaoAtual = data; 
        
        Debug.Log($"[DialogueSystem] Iniciando dialogo: {interacaoAtual.dialogueID}. Tem alternativas? {interacaoAtual.temFrasesAlternativas}");
        
            if (interacaoAtual.temFrasesAlternativas && GameManager != null)
            {
                // Por padrao a condicao e TRUE, a menos que o JSON diga explicitamente que esperaCondicaoFalsa e true
                bool expectedValue = !interacaoAtual.esperaCondicaoFalsa;
                
                Debug.Log($"[DialogueSystem] Condicao esperada avaliada como: {expectedValue}");

                bool condicaoSatisfeita = false;

            if (!string.IsNullOrEmpty(interacaoAtual.variavelDaCondicao))
            {
                bool result = GameManager.ChecarCondicao(interacaoAtual.variavelDaCondicao);
                Debug.Log($"[DialogueSystem] Checando variavel '{interacaoAtual.variavelDaCondicao}'. Resultado: {result}");
                if (result == expectedValue)
                {
                    condicaoSatisfeita = true;
                }
            }
            
            if (!string.IsNullOrEmpty(interacaoAtual.itemDaCondicao))
            {
                bool result = GameManager.ChecarItem(interacaoAtual.itemDaCondicao);
                Debug.Log($"[DialogueSystem] Checando item '{interacaoAtual.itemDaCondicao}'. Resultado: {result}");
                if (result == expectedValue)
                {
                    condicaoSatisfeita = true;
                }
            }

            Debug.Log($"[DialogueSystem] Condicao final satisfeita? {condicaoSatisfeita}");

            if (condicaoSatisfeita)
            {
                dialogueLines = interacaoAtual.falasAlternativas.ToArray(); 
                usouFalasAlternativas = true;
            }
            else
            {
                dialogueLines = interacaoAtual.falas.ToArray(); 
                usouFalasAlternativas = false;
            }
        }
        else
        {
            dialogueLines = interacaoAtual.falas.ToArray(); 
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
