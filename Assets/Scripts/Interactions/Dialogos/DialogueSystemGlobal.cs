using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [Header("Falante (opcional, formato v2)")]
    // Nome de quem fala; some nas falas sem personagem
    [SerializeField] private TextMeshProUGUI nomeText;
    // Retrato do falante (CharacterData em Resources/Personagens); some quando nao ha sprite
    [SerializeField] private Image retratoImage;

    [Header("Efeitos de Transicao")]
    // CanvasGroup do Panel usado no fade de saida
    [SerializeField] private CanvasGroup canvasGroup;
    // Duracao do fade de saida em segundos
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Integracao de Escolhas")]
    // Referencia ao sistema global de escolhas; tambem fornece o GlobalStateManager e a mochila para as Condicoes/Efeitos
    [SerializeField] private ChoiceSystemGlobal choiceSystem;

    [Header("Input Actions")]
    // Acao de input configurada para avancar ou acelerar falas (Enter, Botao A, Click)
    [SerializeField] private InputActionReference submitAction;

    // Falas do dialogo atual ja filtradas (normais, alternativas ou v2)
    private List<Fala> dialogueLines = new List<Fala>();
    // Indice da FALA atual (apesar do nome, nao e letra)
    private int letraAtual = 0;
    // Travas: digitando a linha atual / fazendo o fade de saida
    private bool isTyping = false;
    private bool isTransitioning = false;

    // Variavel publica que sinaliza se o jogador ja pode pressionar o botao para avancar a fala
    // Zerada em IniciarDialogo e liberada 0.2s depois por EsperaEHabilitaAvanco
    public bool podeAvancar = false;

    // Dados do dialogo em andamento (o ultimo da cadeia, quando ha proximoDialogo)
    private DialogueData interacaoAtual;
    // true quando as condicoes bateram e as falas alternativas foram usadas
    private bool usouFalasAlternativas = false;
    // Callback de quem abriu o dialogo (DialogueTrigger destrava o player, CutsceneManager segue a rotina)
    private System.Action onDialogueEndedCallback;
    // Corrotina de digitacao atual (guardada para poder pular a linha)
    private Coroutine typingCoroutine;
    // Cor original do nome, usada quando o personagem nao tem CharacterData
    private Color corNomePadrao = Color.white;

    private GlobalStateManager Estado => choiceSystem != null ? choiceSystem.Estado : null;
    private InventoryData Inventario => choiceSystem != null ? choiceSystem.Inventario : null;

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
        if (nomeText != null) corNomePadrao = nomeText.color;
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
                if (dialogueText != null && letraAtual < dialogueLines.Count)
                {
                    dialogueText.text = TrocarNome(dialogueLines[letraAtual].texto);
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
        if (dialogueLines.Count > 0)
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

    // Avanca a fala; na ultima aplica os efeitos e abre as escolhas ou faz o fade de saida
    private void ProximoDialogo()
    {
        letraAtual++;
        if (letraAtual < dialogueLines.Count)
        {
            typingCoroutine = StartCoroutine(DigitarLinha());
            return;
        }

        if (interacaoAtual != null) Efeito.AplicarTodos(interacaoAtual.efeitos, Estado);

        if (interacaoAtual != null && interacaoAtual.opcoes != null && interacaoAtual.opcoes.Count > 0 && choiceSystem != null)
        {
            AbrirOpcoes();
        }
        else if (interacaoAtual != null && interacaoAtual.terminaEmEscolha && choiceSystem != null)
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
                indice => {
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

    // Formato v2: mostra so as opcoes cujas condicoes valem; no tempo esgotado nenhuma opcao e aplicada
    private void AbrirOpcoes()
    {
        List<Opcao> visiveis = interacaoAtual.opcoes.FindAll(o => o != null && Condicao.Valem(o.condicoes, o.condicoesQualquer, Estado, Inventario));
        if (visiveis.Count == 0)
        {
            Debug.LogWarning($"[DialogueSystem] Nenhuma opcao disponivel em '{interacaoAtual.dialogueID}'; dialogo encerrado.");
            StartCoroutine(Transicao());
            return;
        }

        podeAvancar = false;
        choiceSystem.IniciarTelaDeEscolhas(
            "",
            visiveis.ConvertAll(o => TrocarNome(o.texto)).ToArray(),
            null,
            interacaoAtual.temTempoLimite,
            interacaoAtual.tempoLimite,
            interacaoAtual.escolhaNeutra,
            indice => {
                podeAvancar = true;
                Escolheu(indice >= 0 && indice < visiveis.Count ? visiveis[indice] : null);
            }
        );
    }

    // Aplica os efeitos da opcao e encadeia o proximo dialogo no mesmo Panel (o callback original so roda no fim da cadeia)
    private void Escolheu(Opcao opcao)
    {
        if (opcao != null) Efeito.AplicarTodos(opcao.efeitos, Estado);

        DialogueData proximo = opcao != null ? CarregarDialogo(opcao.proximoDialogo) : null;
        if (proximo != null) Comecar(proximo);
        else StartCoroutine(Transicao());
    }

    // Le Assets/Resources/Dialogos/<nome>.json; null se vazio ou invalido
    public static DialogueData CarregarDialogo(string nome)
    {
        if (string.IsNullOrEmpty(nome)) return null;

        TextAsset json = Resources.Load<TextAsset>("Dialogos/" + nome);
        if (json == null)
        {
            Debug.LogError($"[DialogueSystem] proximoDialogo '{nome}' nao encontrado em Resources/Dialogos.");
            return null;
        }
        try
        {
            return JsonUtility.FromJson<DialogueData>(json.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[DialogueSystem] Erro ao ler Dialogos/{nome}: {ex.Message}");
            return null;
        }
    }

    // Typewriter retro: concatena letra a letra (escolha deliberada, ver MattyAnotacoesDev secao 5)
    // REVIEW: tags de rich text (<color>, <b>) aparecem cruas enquanto digita; se forem usadas, pular a tag inteira de uma vez.
    private IEnumerator DigitarLinha()
    {
        isTyping = true;
        if (dialogueText != null) dialogueText.text = "";

        if (letraAtual < dialogueLines.Count)
        {
            MostrarFalante(dialogueLines[letraAtual]);
            foreach (char letter in TrocarNome(dialogueLines[letraAtual].texto))
            {
                if (dialogueText != null) dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isTyping = false;
    }

    // Atualiza nome e retrato de quem fala (some tudo nas falas sem personagem)
    private void MostrarFalante(Fala fala)
    {
        string personagem = fala.personagem;
        CharacterData dados = string.IsNullOrEmpty(personagem) ? null : Resources.Load<CharacterData>("Personagens/" + personagem);

        if (nomeText != null)
        {
            nomeText.gameObject.SetActive(!string.IsNullOrEmpty(personagem));
            string nome = dados != null && !string.IsNullOrEmpty(dados.nomeExibido) ? dados.nomeExibido : personagem;
            nomeText.text = TrocarNome(nome == "X" ? "{X}" : nome);
            nomeText.color = dados != null ? dados.corDoNome : corNomePadrao;
        }
        if (retratoImage != null)
        {
            Sprite retrato = dados != null ? dados.Retrato(fala.expressao, Estado != null ? Estado.rota : "B") : null;
            retratoImage.sprite = retrato;
            retratoImage.gameObject.SetActive(retrato != null);
        }
    }

    // Troca {X} pelo nome escolhido pelo jogador
    private string TrocarNome(string texto)
    {
        if (string.IsNullOrEmpty(texto)) return "";
        return texto.Replace("{X}", Estado != null ? Estado.NomeExibido : "X");
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

        // Campos do formato antigo; no v2 use "efeitos". Gravam via ChoiceSystemGlobal.SalvarEscolha
        if (interacaoAtual != null && choiceSystem != null)
        {
            if (usouFalasAlternativas)
            {
                if (interacaoAtual.dialogoAlternativoAlteraVariavel)
                    choiceSystem.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogoAlternativo, true);

                if (interacaoAtual.dialogoAlternativoAlteraReputacao)
                    new Efeito { reputacao = interacaoAtual.alvoReputacaoAlternativo, pontos = interacaoAtual.pontosReputacaoAlternativo }.Aplicar(Estado);
            }
            else
            {
                if (interacaoAtual.alteraVariavel)
                    choiceSystem.SalvarEscolha(interacaoAtual.variavelAlteradaPorDialogo, true);

                if (interacaoAtual.alteraReputacao)
                    new Efeito { reputacao = interacaoAtual.alvoReputacao, pontos = interacaoAtual.pontosReputacao }.Aplicar(Estado);
            }
        }

        onDialogueEndedCallback?.Invoke();
        onDialogueEndedCallback = null;

        gameObject.SetActive(false);
    }

    // Ponto de entrada: guarda quem sera avisado no fim e comeca o dialogo
    // Com outro dialogo aberto, substitui o anterior: para as corrotinas dele e o callback antigo nao e chamado
    public void IniciarDialogo(DialogueData data, System.Action onComplete = null)
    {
        if (data == null)
        {
            Debug.LogError("[DialogueSystem] DialogueData recebido e nulo!");
            onComplete?.Invoke();
            return;
        }

        onDialogueEndedCallback = onComplete;
        Comecar(data);
    }

    // Monta as falas (v2 filtradas por condicao, ou normais/alternativas do formato antigo) e comeca a digitar
    private void Comecar(DialogueData data)
    {
        // Sem isso o Enter que abriu o dialogo (Interact e Submit sao a mesma tecla) pode pular a 1a fala
        StopAllCoroutines();
        podeAvancar = false;

        interacaoAtual = data;
        usouFalasAlternativas = false;
        dialogueLines = new List<Fala>();
        if (dialogueText != null) dialogueText.text = "";

        bool formatoV2 = data.linhas != null && data.linhas.Count > 0;
        if (Estado == null && (formatoV2 || (data.opcoes != null && data.opcoes.Count > 0) || (data.efeitos != null && data.efeitos.Count > 0)))
        {
            Debug.LogWarning("[DialogueSystem] Sem ChoiceSystemGlobal/GlobalStateManager: condicoes e efeitos do formato v2 nao vao funcionar.");
        }

        if (formatoV2)
        {
            foreach (var fala in data.linhas)
                if (fala != null && Condicao.Valem(fala.condicoes, fala.condicoesQualquer, Estado, Inventario))
                    dialogueLines.Add(fala);
        }
        else
        {
            List<string> textos = data.falas;
            if (data.temFrasesAlternativas && choiceSystem != null && CondicaoAntigaSatisfeita(data))
            {
                textos = data.falasAlternativas;
                usouFalasAlternativas = true;
            }
            if (textos != null)
                foreach (var texto in textos)
                    dialogueLines.Add(new Fala { texto = texto });
        }

        isTransitioning = false;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
        StartDialogue();
    }

    // Formato antigo: variavelDaCondicao E itemDaCondicao (vazios nao contam), invertidos por esperaCondicaoFalsa
    private bool CondicaoAntigaSatisfeita(DialogueData data)
    {
        bool esperado = !data.esperaCondicaoFalsa;
        if (!string.IsNullOrEmpty(data.variavelDaCondicao) && choiceSystem.ChecarCondicao(data.variavelDaCondicao) != esperado) return false;
        if (!string.IsNullOrEmpty(data.itemDaCondicao) && choiceSystem.ChecarItem(data.itemDaCondicao) != esperado) return false;
        return true;
    }

    // Metodo publico chamado pelo sistema de escolhas ao finalizar uma opcao
    public void EncerrarPosEscolha()
    {
       StartCoroutine(Transicao());
    }
}
