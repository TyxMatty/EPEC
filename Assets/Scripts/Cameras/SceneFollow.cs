using UnityEngine;

public class SceneFollow : MonoBehaviour
    {
    [Header("Target")]
    [SerializeField] private Transform target;
    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    private float alturaCamera; // o quao alta a resolução da camera em unidades unity
    private float larguraCamera; // o quao larga a resolução da camera em unidades unity
    private Vector3 targetPosition; // o x e y da posição que o player vai estar, o z é sempre -10
    void Start()
    {
        alturaCamera = Camera.main.orthographicSize*2f; // a altura da camera é o tamanho ortográfico * 2, porque o tamanho ortográfico é a metade da altura da camera
        // tamanho ortográfico é utilizado para jogos 2D, que é a altura/2 
        larguraCamera = alturaCamera * Camera.main.aspect; // a largura é a altura * aspect ratio(ersolução da altura/resolução da largura)
        
    }
    void LateUpdate()
    {
        float salaX = Mathf.Floor(target.position.x / larguraCamera) * larguraCamera; // o x correto da sala que o player está, arredondado para baixo, multiplicado pela largura da camera
        float salaY = Mathf.Floor(target.position.y / alturaCamera) * alturaCamera; // o y correto da sala que o player está, arredondado para baixo, multiplicado pela altura da camera
        targetPosition = new Vector3(salaX + larguraCamera / 2f, salaY + alturaCamera / 2f, -10f); // o "tp" da camêra, que vai pro centro da sala nova + metade da largura e altura da camera, para ficar no centro da sala
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); // opcional, faz a mudança ser smooth, podia ser:
        // transform.position = targetPosition;  mas ai a camera ia "teleportar" para a nova sala, sem smooth e mei fein
        
    }
}
