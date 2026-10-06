using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Scriptable Object/Item")]
public class ItemData : ScriptableObject 
{
    [Header("ID Permanente (NUNCA MUDE DEPOIS DE CRIADO)")]
    public string itemID; 
    
    [Header("Infos Visuais")]
    public string itemName;
    [TextArea(2, 4)]
    public string itemDescription;
    public Sprite itemIcon;
}