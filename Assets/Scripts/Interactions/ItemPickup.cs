using UnityEngine;
using System.Collections;

// Acao de interacao que permite ao jogador coletar um item e adiciona-lo ao inventario
public class ItemPickup : MonoBehaviour, IInteractAction
{
    [Header("Item do Inventario")]
    // Referencia ao ScriptableObject ItemData que sera adicionado ao inventario do Player
    public ItemData itemToAdd;

    // Metodo chamado pelo InteractionSystem quando o jogador interage com este objeto
    public void OnInteract(InteractionSystem interactor) 
    {
        if (interactor == null) return;

        DialogueTrigger trigger_de_dialogo = GetComponent<DialogueTrigger>();
        
        if (trigger_de_dialogo != null)
        {
            // Se tem dialogo associado, espera o dialogo terminar antes de coletar
            StartCoroutine(EsperarDialogoEPegar(interactor));
        }
        else
        {
            // Se nao tem dialogo, coleta o item diretamente
            PegarItem(interactor);
        }
    }

    private IEnumerator EsperarDialogoEPegar(InteractionSystem interactor)
    {
        if (interactor == null) yield break;

        yield return null;

        // Fica esperando (pausado) enquanto a tela de dialogo estiver ativa
        while (interactor.dialogueSystem != null && interactor.dialogueSystem.gameObject.activeInHierarchy)
        {
            yield return null;
        }

        // qndo o dialogo sumir da tela, a gente finalmente pega o item
        PegarItem(interactor);
    }

    private void PegarItem(InteractionSystem interactor)
    {
        if (interactor == null) return;

        if (itemToAdd == null)
        {
            Debug.LogWarning("[ItemPickup] ItemData nao atribuido no objeto " + gameObject.name);
            return;
        }

        if (interactor.inventarioGlobal != null)
        {
            interactor.inventarioGlobal.Additem(itemToAdd);
            
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[ItemPickup] Inventario nao referenciado no InteractionSystem!");
        }
    }
}
