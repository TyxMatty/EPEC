using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance; // singleton para facil acesso de qualquer lugar

    [Header("Referencias OBRIGATORIAS")]
    public MovementScript playerMovement;
    public DialogueSystemGlobal dialogueSystem;
    
    // (Opcional) Podemos adicionar referencias para controlar Fade In/Fade Out da tela aqui no futuro

    void Awake()
    {
        // Garante que so existe 1 CutsceneManager no jogo
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

    // Le um dialogo JSON de fora e "pausa" a cutscene ate ele acabar
    public IEnumerator TocarDialogoEEsperar(TextAsset dialogueJson)
    {
        if (dialogueJson == null || dialogueSystem == null) 
        {
            Debug.LogError("[CutsceneManager] Falta o JSON ou o Sistema de Dialogo!");
            yield break;
        }

        DialogueData data = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        
        bool dialogoTerminou = false;

        dialogueSystem.gameObject.SetActive(true);
        dialogueSystem.IniciarDialogo(data, () => {
            dialogoTerminou = true;
        });

        // Espera ate o callback avisar que terminou
        while (!dialogoTerminou)
        {
            yield return null; 
        }
    }
}
