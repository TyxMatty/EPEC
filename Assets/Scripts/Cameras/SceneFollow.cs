using UnityEngine;

// Origem: Assets/Scripts/Cameras/SceneFollow.cs
// Script de camera que transiciona entre salas/telas em estilo grid (room-based)
public class SceneFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;

    private float alturaCamera = 10f; 
    private float larguraCamera = 16f; 
    private Vector3 targetPosition; 

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

        float salaX = Mathf.Floor(target.position.x / larguraCamera) * larguraCamera; // o x correto da sala que o player está, arredondado para baixo, multiplicado pela largura da camera
        float salaY = Mathf.Floor(target.position.y / alturaCamera) * alturaCamera; // o y correto da sala que o player está, arredondado para baixo, multiplicado pela altura da camera
        targetPosition = new Vector3(salaX + larguraCamera / 2f, salaY + alturaCamera / 2f, -10f); // o "tp" da camêra, que vai pro centro da sala nova + metade da largura e altura da camera, para ficar no centro da sala
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); // opcional, faz a mudança ser smooth, podia ser:
    }
}
