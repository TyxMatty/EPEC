using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractAction
{
    [Header("Arquivo JSON de Diálogo")] // vamos criar um JSON que tem todos os diálgos aqui
    public TextAsset dialogueJson;

    private DialogueData dataCache; // os atributos do JSON

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
            interactor.dialogueSystem.gameObject.SetActive(true); // se ele for interagivel, e tiver um sistema de diálogo(não pode interagir com algo que n tenha dialogo ne, feedback 101)
            interactor.dialogueSystem.IniciarDialogo(dataCache); // ele inicia a função de iniciar diálogo com o cache do JSON.
            
            // Travar o jogador
            MovementScript moveScript = interactor.GetComponent<MovementScript>();
            if (moveScript != null) moveScript.podeMover = false;
        }
    }
}
