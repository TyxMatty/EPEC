using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]
public class VariavelNarrativa
{
    public string nome;
    public bool valor;
}
public class ChoiceSystemGlobal : MonoBehaviour
{
    [Header("Variáveis Globais de Narrativa")]
    [Header("Variáveis Globais de Narrativa")]
    [Header("Variáveis Globais de Narrativa")]
    [SerializeField] public List<VariavelNarrativa> variaveisGlobais = new List<VariavelNarrativa>();
    [Header("UI de Escolha")]
    [SerializeField] private GameObject choiceUI; // UI de escolha
    [SerializeField] private TMPro.TextMeshProUGUI[] choiceTexts; // Textos
    [SerializeField] private Color corNormal = Color.white;
    [SerializeField] private Color corSelecionada = Color.yellow; // podemos mudar pós-prod
    [SerializeField] private TMPro.TextMeshProUGUI textoTempo; /// futuramente a gente pode mudar por uma barra de tempo, mas por enquanto é só um texto mesmo
    private bool isChoosing = false;
    private int escolhaAtual = -1; // -1 significa que nada foi selecionado ainda
    private string variavelAtualParaSalvar = "";
    private bool usarTempoLimite = false; // se o sistema de escolha vai ter tempo limite ou não
    private float tempoRestante = 0f; // quantidade de tempo restante para o player fazer a escolha
    private bool valorNeutro = false; // valor neutro para quando o tempo acabar e o player não fizer nenhuma escolha
    
    // Array que guarda o valor booleano (true/false) que cada uma das 4 opções representa
    private bool[] valoresDasOpcoes = new bool[4];
    void Start()
    {
        if(choiceUI != null) choiceUI.SetActive(false); // desativa a UI de escolha no início, para evitar conflitos;
        if(textoTempo != null) textoTempo.text = "";
    }
    void Update()
    {
        if (!isChoosing) return;
        if (usarTempoLimite) // timer de escolha, se o tempo acabar, aplica a escolha neutra ou uma escolha diferente
        {
            tempoRestante -= Time.deltaTime;
            
            // Atualiza o texto na tela arredondando para cima (ex: 4.2 vira 5)
            if (textoTempo != null) 
                textoTempo.text = Mathf.CeilToInt(tempoRestante).ToString();

            if (tempoRestante <= 0f)
            {
                Debug.Log($"[ChoiceSystem] Tempo esgotado! Aplicando escolha neutra: {valorNeutro}");
                SalvarEscolha(variavelAtualParaSalvar, valorNeutro);
                EncerrarTelaDeEscolhas();
                return; // Interrompe o Update para não ler inputs
            }
        }

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) MudarSelecao(0);
        else if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame) MudarSelecao(1);
        else if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame) MudarSelecao(2);
        else if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame) MudarSelecao(3);

        if (keyboard.enterKey.wasPressedThisFrame && escolhaAtual != -1)
        {
            ConfirmarEscolha();
        }
    }
    private void MudarSelecao(int direcao) // o metodo que seleciona
    {
        // 0 = cima, 1 = baixo, 2 = esquerda, 3 = direita
      if (choiceTexts[direcao] != null && choiceTexts[direcao].gameObject.activeSelf) // verifica se a opção existe(pode ter casos que seja só A ou B) e tá ativa
        {
            escolhaAtual = direcao;
            
            for (int i = 0; i < choiceTexts.Length; i++)
            {
                if (choiceTexts[i] != null)
                {
                    choiceTexts[i].color = (i == escolhaAtual) ? corSelecionada : corNormal;
                }
            }
        }
    }
    private void ConfirmarEscolha()
    {
        bool valorEscolhido = valoresDasOpcoes[escolhaAtual]; // pega o valor booleano da opção selecionada
        SalvarEscolha(variavelAtualParaSalvar, valorEscolhido); // salva a escolha na variável global
        Debug.Log($"[ChoiceSystem] Escolha confirmada: {escolhaAtual} com valor: {ChecarCondicao(variavelAtualParaSalvar)}");
        EncerrarTelaDeEscolhas(); // encerra a tela de escolhas e retorna o controle para o jogador
        DialogueSystemGlobal dialogueSystem = FindAnyObjectByType<DialogueSystemGlobal>(); 
        dialogueSystem.podeAvançar = true; // libera o player a avançar o diálogo, caso ele queira continuar a conversa com o objeto
    }
    private void EncerrarTelaDeEscolhas()
    {
        isChoosing = false; // desativa choosing
        escolhaAtual = -1; // reseta pra proxima vez que o script rodar
        usarTempoLimite = false; // idem do acima
        tempoRestante = 0f; // idem do acima

        if (choiceUI != null) choiceUI.SetActive(false); // procedimento, desativa a UI de escolha
        if (textoTempo != null) textoTempo.text = ""; // reseta o texto do timer
        DialogueSystemGlobal dialogueSystem = FindAnyObjectByType<DialogueSystemGlobal>();
        dialogueSystem.EncerrarPosEscolha(); // opcional caso tenha uma escolha, para o sistema de diálogo continuar a conversa com o objeto
    }
    private void SalvarEscolha(string nomeVariavel, bool valor)
    {
    // Procura se a variável já existe na lista
    VariavelNarrativa varExistente = variaveisGlobais.Find(v => v.nome == nomeVariavel);

    if (varExistente != null)
        {
            varExistente.valor = valor; // Atualiza se já existir
            Debug.Log($"Variável '{nomeVariavel}' atualizada para: {valor}");
        }
        else
        {
        // Cria uma nova se não existir
            variaveisGlobais.Add(new VariavelNarrativa { nome = nomeVariavel, valor = valor });
            Debug.Log($"Variável '{nomeVariavel}' adicionada com valor: {valor}");
        }
    }
    public bool ChecarCondicao(string nomeVariavel)
    {
    VariavelNarrativa varExistente = variaveisGlobais.Find(v => v.nome == nomeVariavel);

        if (varExistente != null)
        {
        return varExistente.valor;
        }
        else
        {
        Debug.LogWarning($"Variável '{nomeVariavel}' não encontrada. Assumindo false.");
        return false;
        }
    }
    
    
    public void IniciarTelaDeEscolhas(string nomeDaVariavel, string[] textos, bool[] valoresCorrespondentes, bool temTempo = false, float tempo = 0f, bool escolhaNeutra = false)
    {
        variavelAtualParaSalvar = nomeDaVariavel;
        isChoosing = true;
        escolhaAtual = -1; 
        
        usarTempoLimite = temTempo;
        tempoRestante = tempo;
        valorNeutro = escolhaNeutra;

        for (int i = 0; i < 4; i++) // para cada uma das 4 opções,
        {
            if (i < textos.Length && !string.IsNullOrEmpty(textos[i])) // verifica se o texto da opção existe e não é nulo ou vazio
            {
                choiceTexts[i].text = textos[i]; // atualiza o texto da opção
                choiceTexts[i].gameObject.SetActive(true); // ativa a opção
                choiceTexts[i].color = corNormal; // reseta a cor da opção
                valoresDasOpcoes[i] = valoresCorrespondentes[i]; // atualiza o valor booleano da opção
            }
            else if (choiceTexts[i] != null)
            {
                choiceTexts[i].gameObject.SetActive(false); // desativa a opção
            }
        }

        if (choiceUI != null) choiceUI.SetActive(true);
    }
}   

