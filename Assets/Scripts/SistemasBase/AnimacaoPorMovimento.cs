using UnityEngine;

// Origem: Assets/Scripts/SistemasBase/AnimacaoPorMovimento.cs
// Substituto do SpriteChange quando o personagem tiver sprites: alimenta o Animator pela velocidade do Rigidbody2D.
// Serve para player (MovementScript) e NPCs (MovimentoNPC). O controller vem do menu EPEC > Criar Animator Base
// (Humano4Dir / Quadrupede4Dir) via AnimatorOverrideController do personagem. Passo a passo em Docs/README.md.
[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class AnimacaoPorMovimento : MonoBehaviour
{
    // Mesmos nomes dos parametros criados pelo GeradorAnimatorBase
    private static readonly int DirX = Animator.StringToHash("DirX");
    private static readonly int DirY = Animator.StringToHash("DirY");
    private static readonly int Velocidade = Animator.StringToHash("Velocidade");

    private Animator animator;
    private Rigidbody2D rb;
    // Parado continua virado para onde andou por ultimo (comeca olhando para baixo)
    private Vector2 ultimaDirecao = Vector2.down;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 v = rb.linearVelocity;
        if (v.sqrMagnitude > 0.0001f) ultimaDirecao = v.normalized;

        animator.SetFloat(DirX, ultimaDirecao.x);
        animator.SetFloat(DirY, ultimaDirecao.y);
        animator.SetFloat(Velocidade, v.magnitude);
    }
}
