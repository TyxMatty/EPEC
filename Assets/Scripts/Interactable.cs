using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Diálogo do Objeto")]
    [TextArea(3, 5)]
    public string[] falas; 
    
    [Header("Variáveis pós-interação (Opcional)")]
    public bool alteraVariavel; // se o diálogo altera uma variável, só por interagir, ao cnotrário de uma escolha
    public string variavelAlteradaPorDialogo; // a variável que ele vai alterar (Ex: "seAlimentou")

    [Header("Configurações de Escolha (Final do Diálogo)")]
    public bool terminaEmEscolha; // Marca se o diálogo abre uma escolha no fim
    public string variavelParaSalvar; // Ex: "acordouPrimeiroDia"
    
    [Header("Diálogo Condicional (Opcional)")]
    public bool temFrasesAlternativas;
    public string variavelDaCondicao; // a variável que ele vai checar (Ex: "seAlimentou")
    public string itemDaCondicao; // o item que ele vai checar, pode ser o de cima e esse também;
    public bool valorEsperado = true; // só muda para o texto alternativo se a variável for igual a este valor
    [TextArea(3, 5)]
    public string[] falasAlternativas;

    // arrays para as 4 opções (0=W, 1=A, 2=D, 3=S). Deixe vazio as que não usar.
    public string[] textosDasOpcoes = new string[4]; 
    public bool[] valoresDasOpcoes = new bool[4];

    [Header("Configurações de Tempo (Opcional)")]
    public bool temTempoLimite;
    public float tempoLimite = 5f;
    public bool escolhaNeutra; // vvalor salvo se o tempo se esgotar, pode ser negativo ou positivo, 

    [Header("Tipos de Objeto")]
    public bool hasSpecialInteraction; 
    public bool isNonPickableObject; 
    public bool isNPC; 
    public bool isPickableObject; 
    [Header("Inventário")]
    public ItemData itemToAdd; // item que será adicionado ao inventário, se for um objeto coletável
}