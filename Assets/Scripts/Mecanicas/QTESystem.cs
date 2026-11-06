using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Origem: Assets/Scripts/Mecanicas/QTESystem.cs
// QTE generico: mostra uma direcao por vez e o jogador aperta Navigate (WASD/setas) nela dentro da janela de tempo.
// Fica no Canvas do Nucleo. Chamar de uma cutscene com o player travado:
//   yield return QTESystem.Instance.Rodar(5, 1.2f, 1);   // agressor: estado.SetVariavel("agressorEscapa", !Passou)
//   yield return QTESystem.Instance.Rodar(8, 1f, 3);     // ataque de panico: mais de 3 falhas = desmaio
public class QTESystem : MonoBehaviour
{
    public static QTESystem Instance;

    [Header("UI")]
    public GameObject painel;
    // Mostra a direcao pedida
    public TMPro.TextMeshProUGUI textoTecla;
    // Opcional: Image Filled que esvazia com o tempo
    public Image barraTempo;
    // Texto de cada direcao: cima, baixo, esquerda, direita (troque por setas se a fonte tiver)
    public string[] rotulos = { "CIMA", "BAIXO", "ESQUERDA", "DIREITA" };
    public Color corNormal = Color.white;
    public Color corAcerto = Color.green;
    public Color corErro = Color.red;
    // Pausa entre um pedido e outro (mostra a cor de acerto/erro)
    public float intervalo = 0.3f;

    [Header("Input")]
    [SerializeField] private InputActionReference navigateAction;

    // Resultado do ultimo Rodar
    public int Falhas { get; private set; }
    public bool Passou { get; private set; }

    private static readonly Vector2[] Direcoes = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Awake()
    {
        Instance = this;
        if (painel != null) painel.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // 'quantidade' pedidos de 'janela' segundos; para antes se passar de 'falhasMaximas'. Passou = falhas <= falhasMaximas
    public IEnumerator Rodar(int quantidade, float janela, int falhasMaximas)
    {
        if (navigateAction == null || navigateAction.action == null)
        {
            Debug.LogError("[QTESystem] Falta o navigateAction; QTE contado como falho.");
            Falhas = falhasMaximas + 1;
            Passou = false;
            yield break;
        }
        navigateAction.action.Enable();
        Falhas = 0;
        if (painel != null) painel.SetActive(true);

        for (int i = 0; i < quantidade && Falhas <= falhasMaximas; i++)
        {
            int pedida = Random.Range(0, Direcoes.Length);
            if (textoTecla != null)
            {
                textoTecla.text = pedida < rotulos.Length ? rotulos[pedida] : Direcoes[pedida].ToString();
                textoTecla.color = corNormal;
            }

            // shortcut: segurar uma direcao e apertar outra nao conta como novo aperto (WasPressedThisFrame); basta para QTE de toques
            bool acertou = false;
            float tempo = janela;
            while (tempo > 0f)
            {
                tempo -= Time.deltaTime;
                if (barraTempo != null) barraTempo.fillAmount = tempo / janela;
                if (navigateAction.action.WasPressedThisFrame())
                {
                    acertou = Vector2.Dot(navigateAction.action.ReadValue<Vector2>().normalized, Direcoes[pedida]) > 0.7f;
                    break;
                }
                yield return null;
            }

            if (!acertou) Falhas++;
            if (textoTecla != null) textoTecla.color = acertou ? corAcerto : corErro;
            yield return new WaitForSeconds(intervalo);
        }

        Passou = Falhas <= falhasMaximas;
        if (painel != null) painel.SetActive(false);
    }
}
