using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Origem: Assets/Scripts/Mecanicas/QuadroInvestigacao.cs
// Tela de "juntar as pontas": lista as pistas visiveis do caso, Submit marca/desmarca, "Juntar" testa a combinacao.
// Fica no Canvas do Nucleo. Chamar de uma cutscene com o player travado:
//   yield return QuadroInvestigacao.Instance.Investigar(casoJson);
//   if (QuadroInvestigacao.Instance.UltimaConclusao == null) { /* jogador escolheu Voltar */ }
public class QuadroInvestigacao : MonoBehaviour
{
    public static QuadroInvestigacao Instance;

    [Header("Estado")]
    public GlobalStateManager estado;
    // Opcional: so precisa se alguma pista usar {"item": ...}
    public InventoryData mochila;

    [Header("UI")]
    public GameObject painel;
    public TMPro.TextMeshProUGUI titulo;
    // Uma linha por pista + "Juntar" + "Voltar": precisa de (pistas do maior caso + 2) linhas
    public TMPro.TextMeshProUGUI[] linhas;
    // Mostra o erro ou o texto da conclusao
    public TMPro.TextMeshProUGUI resposta;
    public Color corNormal = Color.white;
    public Color corCursor = Color.yellow;

    [Header("Input")]
    // Navigate (W/S, setas) move o cursor; Submit (Enter) marca, junta ou volta
    [SerializeField] private InputActionReference navigateAction;
    [SerializeField] private InputActionReference submitAction;

    // Conclusao achada no ultimo Investigar (null = saiu pelo Voltar)
    public Conclusao UltimaConclusao { get; private set; }

    private List<Pista> visiveis = new List<Pista>();
    private readonly List<string> marcadas = new List<string>();
    private int cursor;

    void Awake()
    {
        Instance = this;
        if (painel != null) painel.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public IEnumerator Investigar(TextAsset casoJson)
    {
        UltimaConclusao = null;
        CasoData caso = casoJson != null ? JsonUtility.FromJson<CasoData>(casoJson.text) : null;
        if (caso == null || linhas == null || linhas.Length < 3 || navigateAction == null || submitAction == null)
        {
            Debug.LogError("[QuadroInvestigacao] Falta o JSON do caso, as linhas da UI (minimo 3) ou os InputActions.");
            yield break;
        }
        navigateAction.action.Enable();
        submitAction.action.Enable();

        visiveis = caso.pistas.FindAll(p => p != null && Condicao.Valem(p.condicoes, null, estado, mochila));
        if (visiveis.Count > linhas.Length - 2)
        {
            Debug.LogWarning($"[QuadroInvestigacao] {visiveis.Count} pistas mas so {linhas.Length - 2} linhas livres; as extras foram cortadas.");
            visiveis.RemoveRange(linhas.Length - 2, visiveis.Count - (linhas.Length - 2));
        }
        marcadas.Clear();
        cursor = 0;

        if (painel != null) painel.SetActive(true);
        if (titulo != null) titulo.text = caso.titulo;
        if (resposta != null) resposta.text = "";
        Desenhar();

        // Pula o frame de abertura: o Enter que fechou o dialogo anterior ainda conta como apertado
        yield return null;
        int juntar = visiveis.Count, voltar = visiveis.Count + 1;
        while (true)
        {
            if (navigateAction.action.WasPressedThisFrame())
            {
                float y = navigateAction.action.ReadValue<Vector2>().y;
                if (y > 0.5f) cursor = (cursor + voltar) % (voltar + 1);
                else if (y < -0.5f) cursor = (cursor + 1) % (voltar + 1);
                Desenhar();
            }

            if (submitAction.action.WasPressedThisFrame())
            {
                if (cursor == voltar) break;
                if (cursor == juntar)
                {
                    Conclusao achada = caso.Encontrar(marcadas);
                    if (achada != null)
                    {
                        UltimaConclusao = achada;
                        Efeito.AplicarTodos(achada.efeitos, estado);
                        if (resposta != null) resposta.text = achada.texto;
                        break;
                    }
                    if (resposta != null) resposta.text = caso.erro;
                }
                else
                {
                    string id = visiveis[cursor].id;
                    if (!marcadas.Remove(id)) marcadas.Add(id);
                }
                Desenhar();
            }
            yield return null;
        }

        // Deixa a conclusao na tela ate o proximo Enter
        if (UltimaConclusao != null)
        {
            yield return null;
            while (!submitAction.action.WasPressedThisFrame()) yield return null;
        }
        if (painel != null) painel.SetActive(false);
    }

    private void Desenhar()
    {
        for (int i = 0; i < linhas.Length; i++)
        {
            if (linhas[i] == null) continue;
            bool usada = i <= visiveis.Count + 1;
            linhas[i].gameObject.SetActive(usada);
            if (!usada) continue;

            if (i < visiveis.Count) linhas[i].text = (marcadas.Contains(visiveis[i].id) ? "[x] " : "[ ] ") + visiveis[i].texto;
            else linhas[i].text = i == visiveis.Count ? "Juntar" : "Voltar";
            linhas[i].color = i == cursor ? corCursor : corNormal;
        }
    }
}
