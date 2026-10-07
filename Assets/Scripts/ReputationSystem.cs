using UnityEngine;
using TMPro;
using System.Collections;

// Sistema gerenciador de reputacao com NPCs/animais e exibicao de popups visuais de afinidade
public class ReputationSystem : MonoBehaviour
{
    // Instancia estatica singleton para facilitar acesso global
    public static ReputationSystem Instance;

    [Header("Base de Dados")]
    // Referencia ao ScriptableObject GlobalStateManager que armazena os pontos persistentes
    public GlobalStateManager stateManager;

    [Header("UI Feedback (Opcional)")]
    // Componente TextMeshProUGUI que exibe na tela o feedback (+10 Afinidade com Tobias)
    public TextMeshProUGUI popupText;
    // Tempo em segundos que o popup permanece visivel na tela
    public float tempoDoPopup = 2f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (popupText != null) popupText.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void AdicionarReputacao(string alvo, int pontos) // 
    {
        if (stateManager != null)
        {
            stateManager.AddReputacao(alvo, pontos);
            Debug.Log($"[ReputationSystem] {pontos} pontos adicionados/removidos para {alvo}. Total agora: {stateManager.GetReputacao(alvo)}");

            if (popupText != null)
            {
                StopAllCoroutines();
                StartCoroutine(ShowPopup(alvo, pontos));
            }
        }
        else
        {
            Debug.LogError("[ReputationSystem] GlobalStateManager nao esta atribuido!");
        }
    }

    private IEnumerator ShowPopup(string alvo, int pontos)
    {
        popupText.gameObject.SetActive(true);
        
        string sinal = pontos > 0 ? "+" : "";
        Color cor = pontos > 0 ? Color.green : Color.red;

        popupText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(cor)}>{sinal}{pontos}</color> Afinidade com {alvo}";

        yield return new WaitForSeconds(tempoDoPopup);

        popupText.gameObject.SetActive(false);
    }
}
