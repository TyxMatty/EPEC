using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Scriptable Objects/Inventory")]
public class InventoryData : ScriptableObject
{
    [Header("Itens Guardados")]
    public List<ItemData> itens = new List<ItemData>();
    public void Additem(ItemData itemAdicionado)
    {
        if (itemAdicionado != null)
        {
            itens.Add(itemAdicionado);
            Debug.Log($"[Inventory]Item adicionado: {itemAdicionado.itemName} foi adicionado ao inventário.");
        }
    }
    public void RemoveItem(ItemData itemRemovido)
    {
        if (itemRemovido != null && itens.Contains(itemRemovido))
        {
            itens.Remove(itemRemovido);
            Debug.Log($"[Inventory]Item removido: {itemRemovido.itemName} foi removido do inventário.");
        }
    }
}
