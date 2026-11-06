using System.Collections;
using TMPro;
using UnityEngine;

// Origem: Assets/Scripts/SistemasBase/NomeDoJogadorUI.cs
// Painel "Como voce se chama?": grava o nome no GlobalStateManager, que substitui {X} nos dialogos
public class NomeDoJogadorUI : MonoBehaviour
{
    [Header("Referencias")]
    // Asset onde o nome fica salvo (vai para o save junto com as flags)
    [SerializeField] private GlobalStateManager stateManager;
    // Painel inteiro da tela de nome (comeca escondido)
    [SerializeField] private GameObject painel;
    // Campo onde o jogador digita; Enter no campo confirma
    [SerializeField] private TMP_InputField campoNome;

    [Header("Configuracao")]
    // Limite de caracteres do nome (cabe na caixa de dialogo)
    [SerializeField] private int tamanhoMaximo = 16;

    private bool confirmado;

    // true enquanto o jogador ainda nao escolheu um nome (jogo novo)
    public bool FaltaNome => stateManager != null && string.IsNullOrEmpty(stateManager.nomeJogador);

    void Awake()
    {
        if (painel != null) painel.SetActive(false);
        if (campoNome != null)
        {
            campoNome.characterLimit = tamanhoMaximo;
            campoNome.onSubmit.AddListener(_ => Confirmar());
        }
    }

    // Para cutscenes: abre o painel e espera o jogador confirmar um nome
    public IEnumerator PedirNome()
    {
        if (stateManager == null || campoNome == null)
        {
            Debug.LogWarning("[NomeDoJogadorUI] stateManager ou campoNome nao atribuidos no Inspector; o jogador fica como X.");
            yield break;
        }

        confirmado = false;
        if (painel != null) painel.SetActive(true);
        campoNome.text = stateManager.nomeJogador;
        campoNome.ActivateInputField();

        while (!confirmado) yield return null;

        if (painel != null) painel.SetActive(false);
    }

    // Ligar no OnClick do botao "Confirmar"; nome vazio e ignorado
    public void Confirmar()
    {
        if (campoNome == null || stateManager == null) return;

        // Sem < e > o nome nao vira tag de rich text nas caixas de dialogo
        string nome = campoNome.text.Replace("<", "").Replace(">", "").Trim();
        if (nome.Length == 0) return;

        stateManager.nomeJogador = nome;
        confirmado = true;
    }
}
