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

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction; // para direcao de olhar
    [SerializeField] private InputActionReference interactAction; // para apertar E/A

    private Transform objectToInteractWith;
    private Vector2 interactionDirection = Vector2.down;
    
    void OnEnable()
    {
        moveAction?.action.Enable();
        interactAction?.action.Enable();
    }

    void OnDisable()
    {
        moveAction?.action.Disable();
        interactAction?.action.Disable();
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
        if(dialogueSystem != null && dialogueSystem.gameObject.activeSelf) return; 

        VerificarInteracao();
        AtualizarDirecaoInteracao(); 
        
        if (interactAction != null && interactAction.action.WasPressedThisFrame() && objectToInteractWith != null)
        {
            Interact();
        }
    }

    private void AtualizarDirecaoInteracao()
    {
        if (moveAction == null) return;
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
            Debug.LogWarning($"O objeto {objectToInteractWith.name} nuo possui aes de interauo configuradas.");
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
