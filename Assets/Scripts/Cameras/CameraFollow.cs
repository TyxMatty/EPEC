using UnityEngine;

// Origem: Assets/Scripts/Cameras/CameraFollow.cs
// Script de camera para seguir o alvo suavemente em espaco 2D
// REVIEW: o README cita limites min/max em X e Y que nao existem aqui. Desativado na cena 0 (quem roda e o SceneFollow).
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    
    // O valor Z deve ser negativo (-10) para a camera ficar atras do cenario 2D
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    // O LateUpdate e usado para cameras porque executa DEPOIS do Update normal do jogador
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;
    }
}