using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

// Sistema central de exibicao de dialogos, animacao typewriter de texto e gatilho de escolhas
public class DialogueSystemGlobal : MonoBehaviour
{
    [Header("Configuracoes do Sistema de Dialogo")]
    // Segundos entre cada letra do typewriter
    [SerializeField] private float typingSpeed = 0.05f;
    // Indicador opcional (nulo na cena 0)
    // REVIEW: so e ligado no fim do dialogo (Transicao) e nunca desligado; se for a setinha de continuar, ligar quando a linha termina.
    [SerializeField] private Transform nextDialogueBox;
    // Texto TMP onde as falas sao digitadas
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [Header("Efeitos de Transicao")] 
    // CanvasGroup do Panel usado no fade de saida
    [SerializeField] private CanvasGroup canvasGroup; 
    // Duracao do fade de saida em segundos
    [SerializeField] private float fadeDuration = 0.5f; 
    
    [Header("Integracao de Escolhas")]
    // Referencia ao sistema global de escolhas para iniciar telas de selecao
    [SerializeField] private ChoiceSystemGlobal choiceSystem; 

    [Header("Input Actions")]
    // Acao de input configurada para avancar ou acelerar falas (Enter, Botao A, Click)
    [SerializeField] private InputActionReference submitAction; 
    
    // Falas do dialogo atual (normais ou alternativas)
    private string[] dialogueLines;
    // Indice da FALA atual (apesar do nome, nao e letra)
    private int letraAtual = 0; 
    // Travas: digitando a linha atual / fazendo o fade de saida
    private bool isTyping = false; 
    private bool isTransitioning = false; 

    // Variavel publica que sinaliza se o jogador ja pode pressionar o botao para avancar a fala
    // Zerada em IniciarDialogo e liberada 0.2s depois por EsperaEHabilitaAvanco
    public bool podeAvancar = false;
    
    // Dados do dialogo em andamento
    private DialogueData interacaoAtual; 
    // true quando as condicoes bateram e as falas alternativas foram usadas
    private bool usouFalasAlternativas = false;
    // Callback de quem abriu o dialogo (DialogueTrigger destrava o player, CutsceneManager segue a rotina)
    private System.Action onDialogueEndedCallback;
    // Corrotina de digitacao atual (guardada para poder pular a linha)
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

    // REVIEW: o Panel comeca ativo na cena 0.unity e nada o esconde aqui; sem uma cutscene abrindo dialogo no inicio,
    // o InteractionSystem fica bloqueado (ele so roda com o Panel inativo).
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

    // Enter durante a digitacao completa a linha; depois dela avanca para a proxima fala
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

    // Comeca da fala 0 e libera o avanco depois de 0.2s (evita pular com o mesmo Enter que abriu)
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

    // Avanca a fala; na ultima abre as escolhas ou faz o fade de saida
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

    // Typewriter retro: concatena letra a letra (escolha deliberada, ver MattyAnotacoesDev secao 5)
    // REVIEW: tags de rich text (<color>, <b>) aparecem cruas enquanto digita; se forem usadas, pular a tag inteira de uma vez.
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

    // Fade de saida, aplica flags/reputacao do dialogo, chama o callback e desliga o Panel
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
        
        // REVIEW: flags e reputacao so sao aplicadas com choiceSystem ligado (gravam via ChoiceSystemGlobal.SalvarEscolha),
        // e sem ReputationSystem na cena o alteraReputacao some sem nenhum aviso.
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

    // Ponto de entrada: escolhe falas normais ou alternativas e comeca a digitar
    // Com outro dialogo aberto, substitui o anterior: para as corrotinas dele e o callback antigo nao e chamado
    public void IniciarDialogo(DialogueData data, System.Action onComplete = null)
    {
        if (data == null)
        {
            Debug.LogError("[DialogueSystem] DialogueData recebido e nulo!");
            onComplete?.Invoke();
            return;
        }

        // Sem isso o Enter que abriu o dialogo (Interact e Submit sao a mesma tecla) pode pular a 1a fala
        StopAllCoroutines();
        podeAvancar = false;

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



