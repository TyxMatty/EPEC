using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance; // Singleton para fácil acesso de qualquer lugar

    [Header("Referencias OBRIGATORIAS")]
    public MovementScript playerMovement;
    public DialogueSystemGlobal dialogueSystem;
    
    // (Opcional) Podemos adicionar referencias para controlar Fade In/Fade Out da tela aqui no futuro

    void Awake()
    {
        // Garante que só existe 1 CutsceneManager no jogo
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TravarJogador()
    {
        if (playerMovement != null) playerMovement.podeMover = false;
    }

    public void LiberarJogador()
    {
        if (playerMovement != null) playerMovement.podeMover = true;
    }

    // lê um diálogo JSON de fora e "pausa" a cutscene até ele acabar
    public IEnumerator TocarDialogoEEsperar(TextAsset dialogueJson)
    {
        if (dialogueJson == null || dialogueSystem == null) 
        {
            Debug.LogError("[CutsceneManager] Falta o JSON ou o Sistema de Dialogo!");
            yield break;
        }

        // Lê o JSON no ar
        DialogueData data = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        
        dialogueSystem.gameObject.SetActive(true);
        dialogueSystem.IniciarDialogo(data);

        // O DialogueSystemGlobal desativa seu próprio GameObject quando a transição do diálogo acaba.
        // Enquanto ele estiver ativo, a cutscene espera.
        while (dialogueSystem.gameObject.activeSelf)
        {
            yield return null; 
        }
    }
}