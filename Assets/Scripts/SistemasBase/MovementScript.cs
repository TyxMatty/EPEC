using UnityEngine;
using UnityEngine.InputSystem;

// Origem: Assets/Scripts/SistemasBase/MovementScript.cs
// Movimento top-down: le Move/Sprint no Update e aplica a velocidade no Rigidbody2D no FixedUpdate
[RequireComponent(typeof(Rigidbody2D))] 
public class MovementScript : MonoBehaviour 
{
    [Header("Speed Configurations")]
    // Velocidade base de caminhada do jogador
    public float baseSpeed = 5f;
    // Multiplicador de velocidade segurando Sprint (Shift)
    [SerializeField] private float shiftMultiplier = 1.5f;

    [Header("Input Actions")]
    // Move (WASD/setas) e Sprint (Shift) do mapa Gameplay
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;

    private Rigidbody2D rb;
    // Direcao e velocidade calculadas no Update e aplicadas no FixedUpdate
    private Vector2 movementDirection;
    private float currentSpeed;

    // Flag publica controlando se o jogador pode se movimentar (bloqueado em dialogos/cutscenes)
    public bool podeMover = true; 

    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Enable();
        }
        if (sprintAction != null && sprintAction.action != null)
        {
            sprintAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Disable();
        }
        if (sprintAction != null && sprintAction.action != null)
        {
            sprintAction.action.Disable();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
        
        rb.gravityScale = 0f; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!podeMover) 
        {
            movementDirection = Vector2.zero;
            currentSpeed = 0f;
            return;
        }

        Vector2 moveInput = Vector2.zero;
        if (moveAction != null && moveAction.action != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }
        
        if (moveInput == Vector2.zero)
        {
            movementDirection = Vector2.zero;
            currentSpeed = 0f;
            return;
        }

        movementDirection = moveInput.normalized;
        bool isSprinting = (sprintAction != null && sprintAction.action != null && sprintAction.action.IsPressed());
        currentSpeed = baseSpeed * (isSprinting ? shiftMultiplier : 1f);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = movementDirection * currentSpeed;
        }
    }
}
