using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(MovementScript))]
public class ScriptChange : MonoBehaviour
{
[Header("Walk Configurations")]
[SerializeField] private Sprite walkUp, walkDown, walkLeft, walkRight;
[Header("Idle Configurations")]
[SerializeField] private Sprite idle1, idle2, idle3, idle4;
[Header("Run Configurations")]
[SerializeField] private Sprite runUp, runDown, runLeft, runRight;
[Header("Transition Configurations")]
[SerializeField] private Sprite transUp, transDown, transLeft, transRight;
[Header("Transition Configurations")]
[SerializeField] private float transitionSpeed = 0.15f;

private float tempoDeMovimento = 0f;
private SpriteRenderer spriteRenderer;
private MovementScript moveScript;
private Rigidbody2D rb;
private float animationTimer = 0f;
private int frameAtual = 0;

    void Start()
    {
        // coloca o componente no cache no início para otimizar
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<MovementScript>();
    }
    void Update()
    {
        Vector2 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;
        if (speed < 0.01f){
            tempoDeMovimento = 0f;
            animationTimer += Time.deltaTime;
            if (animationTimer >= transitionSpeed)
            {
                animationTimer = 0f;
                frameAtual = (frameAtual + 1) % 4; // ou sla quantos idles vão ter, depende da direção artística
                if (frameAtual == 0) spriteRenderer.sprite = idle1;
                else if (frameAtual == 1) spriteRenderer.sprite = idle2;
                else if (frameAtual == 2) spriteRenderer.sprite = idle3;
                else if (frameAtual == 3) spriteRenderer.sprite = idle4;
            }
        }
        else
        {
            frameAtual = 0;
            animationTimer = 0f;
            tempoDeMovimento += Time.deltaTime;
            bool isRunning = speed > moveScript.baseSpeed;
            bool isTransitioning = tempoDeMovimento < transitionSpeed;
            if (isTransitioning)
            {
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                {
                    spriteRenderer.sprite = velocity.x > 0 ? transRight : transLeft;
                }
                else
                {
                    spriteRenderer.sprite = velocity.y > 0 ? transUp : transDown;
                }
            }
            else
            {
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                {
                    spriteRenderer.sprite = isRunning ? (velocity.x > 0 ? runRight : runLeft) : (velocity.x > 0 ? walkRight : walkLeft);
                }
                else
                {
                    spriteRenderer.sprite = isRunning ? (velocity.y > 0 ? runUp : runDown) : (velocity.y > 0 ? walkUp : walkDown);
                }
            }
        }
    }
}
