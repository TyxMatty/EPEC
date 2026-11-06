using UnityEngine;
// Acao de interacao anexada a NPCs e objetos interagiveis para disparar dialogos via JSON
public class DialogueTrigger : MonoBehaviour, IInteractAction
{
    [Header("Arquivo JSON de Dialogo")]
    // Arquivo JSON contendo as falas, condicoes e escolhas do dialogo deste objeto/NPC
    public TextAsset dialogueJson;

    // JSON parseado na primeira interacao e reaproveitado nas proximas
    private DialogueData dataCache;

    // Metodo chamado pelo InteractionSystem quando o jogador aperta a tecla de interacao
    public void OnInteract(InteractionSystem interactor)
    {
        if (interactor == null) return;

        if (dialogueJson == null)
        {
            Debug.LogWarning($"[DialogueTrigger] Nenhum JSON referenciado em {gameObject.name}");
            return;
        }

        if (dataCache == null)
        {
            try
            {
                dataCache = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[DialogueTrigger] Erro ao ler JSON em {gameObject.name}: {ex.Message}");
                return;
            }
        }

        if (interactor.dialogueSystem != null && dataCache != null)
        {
            // Trava o jogador durante a conversa
            MovementScript moveScript = interactor.GetComponent<MovementScript>();
            if (moveScript != null) moveScript.podeMover = false;

            interactor.dialogueSystem.gameObject.SetActive(true); 
            
            // REVIEW: o callback sempre devolve podeMover=true, mesmo se uma cutscene tinha travado o player.
            // (A regra em .agents/rules diz que e o DialogueSystemGlobal que destrava; na pratica e aqui.)
            // Inicia o dialogo passando um callback para destravar o jogador quando acabar
            interactor.dialogueSystem.IniciarDialogo(dataCache, () => {
                if (moveScript != null) moveScript.podeMover = true;
            });
        }
    }
}
