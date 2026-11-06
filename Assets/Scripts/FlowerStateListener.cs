using UnityEngine;

// Origem: Assets/Scripts/FlowerStateListener.cs
// Componente anexado ao GameObject/Prefab da Flor para reagir as mudancas no GlobalStateManager
public class FlowerStateListener : MonoBehaviour
{
    [Header("Referencias Globais")]
    // Referencia ao ScriptableObject GlobalStateManager que armazena as variaveis narrativas
    public GlobalStateManager stateManager;

    [Header("Configuracao da Flor")]
    // Nome da variavel no GlobalStateManager que controla a flor (ex: flor_regada ou flor_viva)
    public string variavelFlor = "flor_regada";

    // Sprite quando a flor esta viva ou regada
    public Sprite spriteViva;

    // Sprite quando a flor esta murcha ou seca
    public Sprite spriteMurcha;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // REVIEW: o SaveManager tambem carrega no Start e nao ha ordem de execucao definida; a flor pode ler o estado de antes do load.
    void Start()
    {
        AtualizarEstadoFlor();
    }

    // Metodo publico chamado para atualizar o visual da flor baseado no estado global
    public void AtualizarEstadoFlor()
    {
        if (stateManager == null || spriteRenderer == null) return;

        bool estaViva = stateManager.GetVariavel(variavelFlor);
        if (estaViva && spriteViva != null)
        {
            spriteRenderer.sprite = spriteViva;
        }
        else if (!estaViva && spriteMurcha != null)
        {
            spriteRenderer.sprite = spriteMurcha;
        }
        // REVIEW: so 2 sprites e so le no Start; as 6 flags florXxxDia1 do GDD vao precisar de uma lista flag->sprite
        // e de alguem chamar AtualizarEstadoFlor quando a flag mudar.
        // podemos adicionar mais futuramente, quando precisar.
    }
}
