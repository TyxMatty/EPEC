using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Diálogo do Objeto")]
    [TextArea(3, 5)]
    public string[] falas; 
    
    [Header("Configurações de Escolha (Final do Diálogo)")]
    public bool terminaEmEscolha; // Marca se o diálogo abre uma escolha no fim
    public string variavelParaSalvar; // Ex: "acordouPrimeiroDia"
    
    // Arrays para as 4 opções (0=W, 1=A, 2=D, 3=S). Deixe vazio as que não usar.
    public string[] textosDasOpcoes = new string[4]; 
    public bool[] valoresDasOpcoes = new bool[4];

    [Header("Configurações de Tempo (Opcional)")]
    public bool temTempoLimite;
    public float tempoLimite = 5f;
    public bool escolhaNeutra; // Valor salvo se o tempo se esgotar, pode ser negativo ou positivo, 

    [Header("Tipos de Objeto")]
    public bool hasSpecialInteraction; 
    public bool isNonPickableObject; 
    public bool isNPC; 
    public bool isPickableObject; 
}