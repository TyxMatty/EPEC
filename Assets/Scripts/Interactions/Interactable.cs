using UnityEngine;

// Origem: Assets/Scripts/Interactions/Interactable.cs
// Tag opcional para categorizacao de objetos do cenario
public class Interactable : MonoBehaviour
{
    [Header("Tipos de Objeto (Opcional para categorizacao futura)")]
    // Sinaliza se o objeto possui algum gatilho especial customizado
    public bool hasSpecialInteraction; 
    // Sinaliza se o objeto e decorativo e nao pode ser coletado para o inventario
    public bool isNonPickableObject; 
    // Sinaliza se o objeto representa um personagem nao-jogavel
    public bool isNPC; 
}
