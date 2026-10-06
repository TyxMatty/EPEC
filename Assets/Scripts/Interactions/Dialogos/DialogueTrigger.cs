using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractAction
{
    [Header("Arquivo JSON de Dialogo")]
    public TextAsset dialogueJson;

    private DialogueData dataCache;

    public void OnInteract(InteractionSystem interactor)
    {
        if (dialogueJson == null)
        {
            Debug.LogWarning($"[DialogueTrigger] Nenhum JSON referenciado em {gameObject.name}");
            return;
        }

        if (dataCache == null)
        {
            dataCache = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        }

        if (interactor.dialogueSystem != null && dataCache != null)
        {
            // Trava o jogador
            MovementScript moveScript = interactor.GetComponent<MovementScript>();
            if (moveScript != null) moveScript.podeMover = false;

            interactor.dialogueSystem.gameObject.SetActive(true); 
            
            // Inicia o dialogo passando um callback para destravar o jogador quando acabar
            interactor.dialogueSystem.IniciarDialogo(dataCache, () => {
                if (moveScript != null) moveScript.podeMover = true;
            });
        }
    }
}
