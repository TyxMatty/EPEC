using UnityEngine;
using System.Collections;

// Origem: Assets/Scripts/CutsceneManager.cs
// Singleton que trava/libera o jogador e toca dialogos JSON esperando o fim, para uso em corrotinas de cutscene
public class CutsceneManager : MonoBehaviour
{
    // Instancia estatica global para acesso rapido das cutscenes sem busca por cena
    public static CutsceneManager Instance;

    [Header("Referencias OBRIGATORIAS")]
    // Referencia ao MovementScript do Player para controlar as travas de locomocao
    public MovementScript playerMovement;
    // Referencia ao sistema global de exibicao de dialogos na tela
    public DialogueSystemGlobal dialogueSystem;
    
    // (Opcional) Podemos adicionar referencias para controlar Fade In/Fade Out da tela aqui no futuro

    void Awake()
    {
        // Garante que so existe 1 CutsceneManager no jogo
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // Metodo publico para travar a movimentacao do jogador durante eventos narrativos
    public void TravarJogador()
    {
        if (playerMovement != null) playerMovement.podeMover = false;
    }

    // Metodo publico para liberar a movimentacao do jogador apos o termino de eventos
    public void LiberarJogador()
    {
        if (playerMovement != null) playerMovement.podeMover = true;
    }

    // Le um dialogo JSON de fora e aguarda a finalizacao antes de prosseguir com a Coroutine
    public IEnumerator TocarDialogoEEsperar(TextAsset dialogueJson)
    {
        if (dialogueJson == null || dialogueSystem == null) 
        {
            Debug.LogError("[CutsceneManager] Falta o JSON ou o Sistema de Dialogo!");
            // REVIEW: so este erro libera o jogador; os de JSON invalido abaixo nao. No meio de uma cutscene isso destrava o player antes da hora.
            LiberarJogador();
            yield break;
        }

        DialogueData data = null;
        try
        {
            data = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[CutsceneManager] Erro ao processar JSON de dialogo: " + ex.Message);
            yield break;
        }

        if (data == null)
        {
            Debug.LogError("[CutsceneManager] DialogueData resultou em nulo.");
            yield break;
        }

        bool dialogoTerminou = false;

        dialogueSystem.gameObject.SetActive(true);
        dialogueSystem.IniciarDialogo(data, () => {
            dialogoTerminou = true;
        });

        // REVIEW: nao ha timeout (os docs falam em tempo limite); a unica guarda e o Panel ficar inativo.
        // Aguarda com verificacao de seguranca caso o sistema de dialogo seja desativado externamente
        yield return null;
        while (!dialogoTerminou)
        {
            if (dialogueSystem == null || !dialogueSystem.gameObject.activeInHierarchy)
            {
                dialogoTerminou = true;
                break;
            }
            yield return null; 
        }
    }
}
