using UnityEngine;
using UnityEngine.InputSystem;

// Sistema do Player responsavel por detectar objetos interagiveis a frente e disparar IInteractAction
public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Configurations")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactionPrompt;

    [Header("Sistemas Globais")]
    // Referencia ao sistema global de dialogos para verificar se a UI de fala esta ativa
    public DialogueSystemGlobal dialogueSystem;
    // Referencia ao inventario do jogador para transferir itens coletados via ItemPickup
    public InventoryData inventarioGlobal;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction; 
    [SerializeField] private InputActionReference interactAction; 

    private Transform objectToInteractWith;
    private Vector2 interactionDirection = Vector2.down;
    
    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Enable();
        }
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Disable();
        }
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.Disable();
        }
    }

    void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {   
        if (dialogueSystem != null && dialogueSystem.gameObject.activeSelf) return; 

        VerificarInteracao();
        AtualizarDirecaoInteracao(); 
        
        if (interactAction != null && interactAction.action != null && interactAction.action.WasPressedThisFrame() && objectToInteractWith != null)
        {
            Interact();
        }
    }

    private void AtualizarDirecaoInteracao()
    {
        if (moveAction == null || moveAction.action == null) return;
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            interactionDirection = input.normalized;
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

        IInteractAction[] actions = objectToInteractWith.GetComponents<IInteractAction>();
        
        if (actions.Length == 0)
        {
            Debug.LogWarning($"O objeto {objectToInteractWith.name} nao possui acoes de interacao configuradas.");
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

