using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Configurations")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactionPrompt;

    [Header("Sistemas Globais")]
    public DialogueSystemGlobal dialogueSystem;
    public InventoryData inventarioGlobal;

    private Transform objectToInteractWith;
    private Vector2 interactionDirection = Vector2.down;
    
    void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {   
        if(dialogueSystem != null && dialogueSystem.gameObject.activeSelf) return; 

        VerificarInteracao();
        AtualizarDirecaoInteracao(); 
        
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        
        if ((keyboard.eKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame) && objectToInteractWith != null)
        {
            Interact();
        }
    }

    private void AtualizarDirecaoInteracao()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float moveX = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
        float moveY = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);

        if (moveX != 0f || moveY != 0f)
        {
            interactionDirection = new Vector2(moveX, moveY).normalized;
        }
    }

    private void VerificarInteracao()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, interactionDirection, interactionRange, interactableLayer);
        
        if (hit.collider != null)
        {
            objectToInteractWith = hit.transform;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
        else
        {
            objectToInteractWith = null;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }

    private void Interact()
    {
        if (objectToInteractWith == null) return;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false); 
        }

        // Executa todas as ações que o objeto possui
        IInteractAction[] actions = objectToInteractWith.GetComponents<IInteractAction>();
        
        if (actions.Length == 0)
        {
            Debug.LogWarning($"O objeto {objectToInteractWith.name} não possui ações de interação configuradas (IInteractAction).");
            return;
        }

        foreach (var action in actions)
        {
            action.OnInteract(this);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(interactionDirection * interactionRange));
    }
}
