using UnityEngine;
using UnityEngine.InputSystem;


// public são variaveis que qualquer script pode acessar,
// private são variaveis que só podem ser acessadas dentro do script, só para vcs saberem

[RequireComponent(typeof(Rigidbody2D))] // garante que o objeto tenha um Rigidbody2D(tenha colisão)
public class MovementScript : MonoBehaviour // public class significa que qualquer script pode acessar esse script(sim é confuso, mas recursão é legal),
//  e MonoBehaviour é o tipo de script que pode ser colocado em um GameObject
{
    [Header("Speed Configurations")]
    [SerializeField] public float baseSpeed = 5f;
    [SerializeField] private float shiftMultiplier = 1.5f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private float currentSpeed;
    public bool podeMover = true; // variável para controlar se o player pode se mover ou não, 
    // para quando o sistema de diálogo estiver ativo, o player não possa se mover; ou em eventos de
    void Start()
    {
        // coloca o componente no cache no início para otimizar
        rb = GetComponent<Rigidbody2D>();
        
        // QUALQUER coisa que mexa com física, como gravidade, colisão, etc, deve ter essas 2 linhas, senão VAI bugar:
        rb.gravityScale = 0f; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float moveX = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
        float moveY = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);

        // sem input, nada acontece
        if (moveX == 0f && moveY == 0f)
        {
            movementDirection = Vector2.zero;
            return;
        }if (!podeMover) // se o player não pode se mover, não deixa ele se mover
        {
            movementDirection = Vector2.zero;
            return;
        }
        movementDirection = new Vector2(moveX, moveY).normalized;
        currentSpeed = baseSpeed * (keyboard.leftShiftKey.isPressed ? shiftMultiplier : 1f);
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementDirection * currentSpeed;
    }
}
