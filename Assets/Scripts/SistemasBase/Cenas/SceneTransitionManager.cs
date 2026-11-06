using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Origem: Assets/Scripts/SistemasBase/Cenas/SceneTransitionManager.cs
// Fica no objeto RAIZ do prefab "Nucleo" (player, camera, Canvas de UI, CutsceneManager, SaveManager...).
// Mantem o Nucleo vivo entre cenas e troca de cena com fade, colocando o player no PontoDeSpawn pedido.
// Toda cena pode ter uma copia do Nucleo para ser testada sozinha: a copia que chega depois se desliga e se apaga.
[DefaultExecutionOrder(-1000)] // Awake antes dos filhos, para a copia se desligar antes de eles acordarem
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Referencias")]
    // Transform do player (filho do Nucleo)
    public Transform jogador;
    // CanvasGroup de uma Image preta que cobre a tela (Canvas com Sort Order alto); vazio = troca sem fade
    public CanvasGroup telaPreta;
    // Duracao de cada metade do fade, em segundos
    public float duracaoFade = 0.5f;

    // true do inicio do fade-out ate o fim do fade-in; portas ignoram pedidos nesse meio tempo
    public bool EmTransicao { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Desligar antes do Destroy impede Awake/OnEnable/OnDisable dos filhos da copia
            // (o OnDisable do MovementScript/InteractionSystem desligaria as InputActions do player de verdade)
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (telaPreta != null)
        {
            telaPreta.alpha = 0f;
            telaPreta.blocksRaycasts = false;
        }
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // Para portas, UnityEvents e cutscenes: troca para 'cena' (precisa estar no Build Profiles) e nasce em 'spawn'
    public void IrPara(string cena, string spawn)
    {
        if (EmTransicao) return;
        if (!Application.CanStreamedLevelBeLoaded(cena))
        {
            Debug.LogError($"[SceneTransitionManager] Cena '{cena}' nao existe ou nao esta na lista de cenas do Build Profiles.");
            return;
        }
        StartCoroutine(Trocar(cena, spawn));
    }

    private IEnumerator Trocar(string cena, string spawn)
    {
        EmTransicao = true;
        if (CutsceneManager.Instance != null) CutsceneManager.Instance.TravarJogador();
        yield return Fade(1f);

        // Libera antes de carregar: o Start da cena nova pode travar de novo (cutscene) e nao pode ser desfeito aqui
        if (CutsceneManager.Instance != null) CutsceneManager.Instance.LiberarJogador();
        yield return SceneManager.LoadSceneAsync(cena);

        ColocarNoSpawn(spawn);
        yield return Fade(0f);
        EmTransicao = false;
    }

    private void ColocarNoSpawn(string spawn)
    {
        if (jogador == null || string.IsNullOrEmpty(spawn)) return;

        foreach (PontoDeSpawn ponto in FindObjectsByType<PontoDeSpawn>())
        {
            if (ponto.id != spawn) continue;

            Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.position = ponto.transform.position;
                rb.linearVelocity = Vector2.zero;
            }
            jogador.position = ponto.transform.position;

            // Sem isso o CameraFollow atravessaria o mapa ate o player durante o fade-in
            Camera cam = Camera.main;
            if (cam != null) cam.transform.position = new Vector3(jogador.position.x, jogador.position.y, cam.transform.position.z);
            return;
        }
        Debug.LogWarning($"[SceneTransitionManager] Nenhum PontoDeSpawn com id '{spawn}' na cena '{SceneManager.GetActiveScene().name}'.");
    }

    private IEnumerator Fade(float alvo)
    {
        if (telaPreta == null) yield break;

        telaPreta.blocksRaycasts = true;
        float passo = 1f / Mathf.Max(0.01f, duracaoFade);
        while (!Mathf.Approximately(telaPreta.alpha, alvo))
        {
            telaPreta.alpha = Mathf.MoveTowards(telaPreta.alpha, alvo, passo * Time.unscaledDeltaTime);
            yield return null;
        }
        telaPreta.blocksRaycasts = alvo > 0f;
    }
}
