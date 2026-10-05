using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Configurations")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer; // layer que o player pode interagir
    [SerializeField] private GameObject interactionPrompt; // Ui de texto

    [Header("Dialogue Script")]
    [SerializeField] private DialogueSystemGlobal dialogueSystem;
    [Header("Inventário do Jogador")]
    [SerializeField] private InventoryData inventarioGlobal;
    private Transform objectToInteractWith;
    private Vector2 interactionDirection = Vector2.down; // começa olhando para baixo, pode zer Vector2.zero, também
    
    void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {   
        if(dialogueSystem != null && dialogueSystem.gameObject.activeSelf) return; // se o sistema de diálogo estiver ativo, não deixa o player interagir com outros objetos
        VerificarInteracao();
        AtualizarDirecaoInteracao(); // direção que o player está olhando 
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

        float moveX = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f); // mesma logica do MovementScript, mas aqui é só para atualizar a direção que o player está olhando, não para movimentar o player
        float moveY = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);

        if (moveX != 0f || moveY != 0f)
        {
            interactionDirection = new Vector2(moveX, moveY).normalized;
        }
    }
    private void VerificarInteracao()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, interactionDirection, interactionRange, interactableLayer); // lança um "raio"
        // na direção que o player está olhando, com o tamanho do range, e só vai detectar objetos na layer de interação, é 2f, pode aumentar dps
        if (hit.collider != null) // se hitar com algo
        {
            objectToInteractWith = hit.transform; // guarda o objeto que o player "hitou"
            if (interactionPrompt != null) // se o objeto tiver um prompt de interação, ativa ele
            {
                interactionPrompt.SetActive(true);
            }
        }
        else
        {
            objectToInteractWith = null; // se não hitar com nada, zera o objeto que o player pode interagir
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
private void Interact()
    {
        if (dialogueSystem == null || objectToInteractWith == null) return;

        MovementScript movementScript = GetComponent<MovementScript>();
        Interactable interactable = objectToInteractWith.GetComponent<Interactable>();
        
        if (interactable == null) return;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false); 
        }

        // 1. SISTEMA DE INVENTÁRIO (Lida com a coleta primeiro)

        if (interactable.isPickableObject && interactable.itemToAdd != null) // 
        {
            if (inventarioGlobal == null)
            {
                Debug.LogWarning("Inventário não referenciado no Inspector!");
                return;
            }

            inventarioGlobal.Additem(interactable.itemToAdd);
            Debug.Log($"Item adicionado ao inventário: {interactable.itemToAdd.itemName}");

            if (interactable.falas == null || interactable.falas.Length == 0)
            {
                Destroy(objectToInteractWith.gameObject);
                return;
            }
            else
            {
                SpriteRenderer sprite = objectToInteractWith.GetComponent<SpriteRenderer>();
                Collider2D col = objectToInteractWith.GetComponent<Collider2D>();
                
                if (sprite != null) sprite.enabled = false;
                if (col != null) col.enabled = false;
            }
        }

        // 2. SISTEMA DE DIÁLOGO
        // futuramente fazer ficar mais escalável.
        if (interactable.falas != null && interactable.falas.Length > 0)
        {
            if (movementScript != null) movementScript.podeMover = false; 
            
            dialogueSystem.gameObject.SetActive(true);
            dialogueSystem.IniciarDialogo(interactable);
            Debug.Log($"Iniciando diálogo com: {objectToInteractWith.name}");
        }
        else if (!interactable.isPickableObject)
        {
            Debug.LogWarning($"O objeto {objectToInteractWith.name} não possui falas nem é coletável.");
        }
    }
    private void OnDrawGizmos() // isso é uma função de testing; players não vão ver isso, pode ser comentado depois, mas é bom deixar pra testar o range de interação do player
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(interactionDirection * interactionRange));
    }
}
