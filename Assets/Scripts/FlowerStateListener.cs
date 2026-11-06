using System.Collections.Generic;
using UnityEngine;

// Origem: Assets/Scripts/FlowerStateListener.cs
// Flor (ou qualquer sprite do cenario) que muda com o estado narrativo. A lista 'estados' vai em ordem de prioridade:
// vale o PRIMEIRO cujas condicoes batem; um ultimo estado sem condicoes serve de padrao.
// Reage sozinha a toda mudanca do GlobalStateManager (evento Mudou), inclusive ao carregar o save.
[RequireComponent(typeof(SpriteRenderer))]
public class FlowerStateListener : MonoBehaviour
{
    [Header("Referencias Globais")]
    public GlobalStateManager stateManager;

    [Header("Estados da Flor")]
    // Ex: florSuperPositivaDia1 -> brilhante, florNegativaDia1 -> quase morta, ..., (sem condicao) -> normal
    public List<EstadoVisual> estados = new List<EstadoVisual>();

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (stateManager != null) stateManager.Mudou += AtualizarEstadoFlor;
        AtualizarEstadoFlor();
    }

    void OnDisable()
    {
        if (stateManager != null) stateManager.Mudou -= AtualizarEstadoFlor;
    }

    // Publico para quem quiser forcar a atualizacao (ex: depois de trocar a lista em codigo)
    public void AtualizarEstadoFlor()
    {
        if (stateManager == null || spriteRenderer == null) return;

        EstadoVisual escolhido = estados.Find(e => e != null && Condicao.Valem(e.condicoes, null, stateManager, null));
        if (escolhido != null && escolhido.sprite != null) spriteRenderer.sprite = escolhido.sprite;
    }
}

[System.Serializable]
public class EstadoVisual
{
    // So para identificar no Inspector (vira o titulo do elemento da lista)
    public string nome;
    public List<Condicao> condicoes = new List<Condicao>();
    public Sprite sprite;
}
