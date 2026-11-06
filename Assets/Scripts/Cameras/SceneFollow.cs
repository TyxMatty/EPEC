using UnityEngine;

// Origem: Assets/Scripts/Cameras/SceneFollow.cs
// Script de camera que transiciona entre salas/telas em estilo grid (room-based)
public class SceneFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;

    // Tamanho de uma sala = area visivel da camera ortografica (recalculado no Start)
    private float alturaCamera = 10f; 
    private float larguraCamera = 16f; 
    private Vector3 targetPosition; 

    // REVIEW: tamanho da sala e lido so no Start; mudar resolucao/aspecto da janela desalinha a grade. As salas comecam em (0,0).
    void Start()
    {
        if (Camera.main != null)
        {
            alturaCamera = Camera.main.orthographicSize * 2f; 
            larguraCamera = alturaCamera * Camera.main.aspect; 
        }
    }

    void LateUpdate()
    {
        if (target == null || larguraCamera <= 0f || alturaCamera <= 0f) return;

        float salaX = Mathf.Floor(target.position.x / larguraCamera) * larguraCamera; // o x correto da sala que o player esta, arredondado para baixo, multiplicado pela largura da camera
        float salaY = Mathf.Floor(target.position.y / alturaCamera) * alturaCamera; // o y correto da sala que o player esta, arredondado para baixo, multiplicado pela altura da camera
        targetPosition = new Vector3(salaX + larguraCamera / 2f, salaY + alturaCamera / 2f, -10f); // o "tp" da camera, que vai pro centro da sala nova + metade da largura e altura da camera, para ficar no centro da sala
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); // opcional, deixa a troca de sala suave; para corte seco use transform.position = targetPosition
    }
}
