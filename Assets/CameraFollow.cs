using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    
    // o valor Z deve ser negativo (-10) para a câmera ficar atrás do cenário 2D
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    // o lateUpdate é usado para câmeras porque corre DEPOIS do Update normal do jogador
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;
    }
}