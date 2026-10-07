using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))] 
public class MovementScript : MonoBehaviour 
{
    [Header("Speed Configurations")]
    [SerializeField] public float baseSpeed = 5f;
    [SerializeField] private float shiftMultiplier = 1.5f;

    [Header("Input Actions (Arraste aqui do Inspector)")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;

    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private float currentSpeed;
    public bool podeMover = true; 

    void OnEnable()
    {
        moveAction?.action.Enable();
        sprintAction?.action.Enable();
    }

    void OnDisable()
    {
        moveAction?.action.Disable();
        sprintAction?.action.Disable();
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

        Vector2 moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        
        if (moveInput == Vector2.zero)
        {
            movementDirection = Vector2.zero;
            currentSpeed = 0f;
            return;
        }

        movementDirection = moveInput.normalized;
        bool isSprinting = sprintAction != null && sprintAction.action.IsPressed();
        currentSpeed = baseSpeed * (isSprinting ? shiftMultiplier : 1f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementDirection * currentSpeed;
    }
}
