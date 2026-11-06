using UnityEngine;

// Contrato de toda acao disparada pelo InteractionSystem (DialogueTrigger, ItemPickup...)
public interface IInteractAction
{
    void OnInteract(InteractionSystem interactor);
}
