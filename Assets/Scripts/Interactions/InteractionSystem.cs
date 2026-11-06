using UnityEngine;
using UnityEngine.InputSystem;

// Sistema do Player responsavel por detectar objetos interagiveis a frente e disparar IInteractAction
public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Configurations")]
    // Alcance do raycast em unidades
    [SerializeField] private float interactionRange = 2f;
    // Layer dos objetos interagiveis (layer 6 na cena 0)
    [SerializeField] private LayerMask interactableLayer;
    // Icone de interacao mostrado quando ha algo na frente
    [SerializeField] private GameObject interactionPrompt;

    [Header("Sistemas Globais")]
    // Referencia ao sistema global de dialogos para verificar se a UI de fala esta ativa
    public DialogueSystemGlobal dialogueSystem;
    // Referencia ao inventario do jogador para transferir itens coletados via ItemPickup
    public InventoryData inventarioGlobal;

    [Header("Input Actions")]
    // Move define a direcao do raycast; Interact dispara (Interact e Submit estao ambos no Enter)
    [SerializeField] private InputActionReference moveAction; 
    [SerializeField] private InputActionReference interactAction; 

    // Objeto atingido pelo raycast neste frame
    private Transform objectToInteractWith;
    // Ultima direcao de movimento (o raycast aponta para ela)
    private Vector2 interactionDirection = Vector2.down;
    // MovementScript do player: podeMover = false (cutscene/dialogo) tambem bloqueia a interacao
    private MovementScript movement;

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

    // REVIEW: Move e o mesmo InputAction do MovementScript; desligar este componente desliga o movimento tambem.
    // Habilitar o mapa Gameplay uma vez (ex: no InputRebindManager) e nao desligar por componente evita isso.
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
        movement = GetComponent<MovementScript>();

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {   
        if (dialogueSystem != null && dialogueSystem.gameObject.activeSelf) return;

        // Player travado por cutscene sem dialogo na tela (ex: pausa de 2s do WakeUpPrototype): sem interacao nem prompt
        if (movement != null && !movement.podeMover)
        {
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            return;
        }

        VerificarInteracao();
        AtualizarDirecaoInteracao(); 
        
        if (interactAction != null && interactAction.action != null && interactAction.action.WasPressedThisFrame() && objectToInteractWith != null)
        {
            Interact();
        }
    }

    // Guarda a ultima direcao de input nao-nula
    private void AtualizarDirecaoInteracao()
    {
        if (moveAction == null || moveAction.action == null) return;
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            interactionDirection = input.normalized;
        }
    }

    // Raycast na direcao atual; liga/desliga o prompt
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

        // Roda todas as acoes do objeto (ex: DialogueTrigger + ItemPickup). O componente Interactable nao e lido aqui.
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

