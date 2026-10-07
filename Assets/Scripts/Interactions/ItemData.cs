using UnityEngine;

// ScriptableObject que define os atributos imutaveis e visuais de um item
[CreateAssetMenu(fileName = "NovoItem", menuName = "Scriptable Object/Item")]
public class ItemData : ScriptableObject 
{
    [Header("ID Permanente (NUNCA MUDE DEPOIS DE CRIADO)")]
    // Identificador de texto unico usado para checagem em dialogos e no sistema de save
    public string itemID; 
    
    [Header("Infos Visuais")]
    // Nome legivel exibido na interface de inventario
    public string itemName;
    [TextArea(2, 4)]
    // Descricao textual de lore/funcao do item
    public string itemDescription;
    // Icone grafico de sprite do item
    public Sprite itemIcon;
}