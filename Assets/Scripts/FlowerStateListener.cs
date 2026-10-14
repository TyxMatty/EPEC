using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class FlowerStateListener : MonoBehaviour
{
    [System.Serializable]
    public struct StateAppearence
    {
        public string nomeVariavel; //quando essa variável estiver verdadeira aplica o sprite e a cor
        public Sprite sprite;
        public Color color;
    }

    [SerializeField] private GlobalStateManager globalStateManager;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Tooltip("Ordem de prioridade")]
    [SerializeField] private List<StateAppearence> stateAppearences = new List<StateAppearence>();

    [Header("Aparência Padrão")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color defaultColor = Color.white;

    private int lastMatchedIndex = -1; //evita reatribuir todo frame

    private void Reset() => spriteRenderer = GetComponent<SpriteRenderer>();
    private void Update() => Refresh();

    private void OnEnable()
    {
        if (globalStateManager == null) Debug.LogWarning($"GlobalStateManager não atribuído em {name}.", this);
        lastMatchedIndex = -1; //força refresh ao reativar
        Refresh();
    }
    private void Refresh()
    {
        if (globalStateManager == null) return;
        //percorre a lista por prioridade e para no primeiro ativo
        int matchedIndex = -1;
        for (int i = 0; matchedIndex < 0 && i < stateAppearences.Count; i++)
            if (EstaAtiva(stateAppearences[i].nomeVariavel)) matchedIndex = i;

        if (matchedIndex == lastMatchedIndex) return;
        lastMatchedIndex = matchedIndex;
        //sem nenhum estado ativo, usa a aparência padrão
        var state = matchedIndex >= 0 ? stateAppearences[matchedIndex] : new StateAppearence { sprite = defaultSprite, color = defaultColor };
        spriteRenderer.sprite = state.sprite != null ? state.sprite : defaultSprite;
        spriteRenderer.color = state.color;
    }
    private bool EstaAtiva(string nome)
    {
        foreach (var v in globalStateManager.variaveisGlobais)
            if (!string.IsNullOrEmpty(nome) && v.nome == nome) return v.valor;
        return false;
    }
}