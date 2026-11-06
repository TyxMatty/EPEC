using System.Collections;
using UnityEngine;

// Origem: Assets/Scripts/SistemasBase/MovimentoNPC.cs
// Move NPCs e animais em cutscenes pela velocidade do Rigidbody2D (assim o AnimacaoPorMovimento anima igual ao player).
// Em corrotina: yield return maria.MoverPara(ponto.position);  Seguir: tobias.SeguirAlvo(player, fezCarinho ? 1f : 2.5f);
[RequireComponent(typeof(Rigidbody2D))]
public class MovimentoNPC : MonoBehaviour
{
    public float velocidade = 3f;

    [Header("Seguir (opcional)")]
    // Alvo seguido quando nao ha MoverPara em andamento; para a 'distancia' dele
    public Transform alvo;
    public float distancia = 1.5f;

    private Rigidbody2D rb;
    private Vector2 destino;
    private bool indo;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Anda ate 'ponto' e termina ao chegar. Preso em algo por 'tempoMaximo' segundos, teleporta (a cutscene nunca trava)
    public IEnumerator MoverPara(Vector2 ponto, float tempoMaximo = 10f)
    {
        destino = ponto;
        indo = true;
        float tempo = 0f;
        while (indo && tempo < tempoMaximo)
        {
            tempo += Time.deltaTime;
            yield return null;
        }
        if (indo)
        {
            Debug.LogWarning($"[MovimentoNPC] {name} nao chegou em {ponto} a tempo; teleportando.");
            Chegar();
        }
    }

    public void SeguirAlvo(Transform novoAlvo, float novaDistancia)
    {
        alvo = novoAlvo;
        distancia = novaDistancia;
    }

    public void PararDeSeguir()
    {
        alvo = null;
    }

    void FixedUpdate()
    {
        Vector2 falta;
        if (indo)
        {
            falta = destino - rb.position;
            if (falta.magnitude <= velocidade * Time.fixedDeltaTime)
            {
                Chegar();
                return;
            }
        }
        else if (alvo != null)
        {
            falta = (Vector2)alvo.position - rb.position;
            if (falta.magnitude <= distancia) falta = Vector2.zero;
        }
        else falta = Vector2.zero;

        rb.linearVelocity = falta == Vector2.zero ? Vector2.zero : falta.normalized * velocidade;
    }

    private void Chegar()
    {
        rb.position = destino;
        rb.linearVelocity = Vector2.zero;
        indo = false;
    }
}
