using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour, IInteractAction
{
    [Header("Item do Inventário")]
    public ItemData itemToAdd;

    public void OnInteract(InteractionSystem interactor) 
    {
        DialogueTrigger trigger_de_dialogo = GetComponent<DialogueTrigger>();
        
        if (trigger_de_dialogo != null)
        {
            // Se tem diálogo, comçamos uma Coroutine que espera o diálogo terminar
            StartCoroutine(EsperarDialogoEPegar(interactor));
        }
        else
        {
            // Se não tem dialogo, pega direto
            PegarItem(interactor);
        }
    }

    private IEnumerator EsperarDialogoEPegar(InteractionSystem interactor)
    {
        // Espera um frame para garantir que o DialogueTrigger ativou a UI do diálogo
        yield return null;

        // Fica esperando (pausado) enquanto a tela de dialogo estiver ativa
        while (interactor.dialogueSystem != null && interactor.dialogueSystem.gameObject.activeInHierarchy)
        {
            yield return null;
        }

        // Quando o dialogo sumir da tela, a gente finalmente pega o item!
        PegarItem(interactor);
    }

    private void PegarItem(InteractionSystem interactor)
    {
        if (interactor.inventarioGlobal != null && itemToAdd != null)
        {
            interactor.inventarioGlobal.Additem(itemToAdd);
            
            gameObject.SetActive(false);
        }
        else if (interactor.inventarioGlobal == null) 
        {
            Debug.LogWarning("Inventário não referenciado no InteractionSystem!");
        }
    }
}
