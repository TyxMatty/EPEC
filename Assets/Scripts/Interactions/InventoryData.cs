using System.Collections.Generic;
using UnityEngine;


// ScriptableObject que armazena a lista de itens atualmente no inventario do Player
// REVIEW: como no GlobalStateManager, itens pegos em Play Mode ficam gravados no asset (Mochila 1 ja tem a Chave commitada).
// Os READMEs citam AdicionarItem/RemoverItem/ContemItem/Limpar; os metodos reais sao Additem/AddItem/RemoveItem.
[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Scriptable Objects/Inventory")]
public class InventoryData : ScriptableObject
{
    [Header("Itens Guardados")]
    // Lista de itens coletados pelo jogador presentes no inventario
    public List<ItemData> itens = new List<ItemData>();

    // Adiciona um item ao inventario
    public void Additem(ItemData itemAdicionado)
    {
        AddItem(itemAdicionado);
    }

    // Metodo padronizado PascalCase para adicionar item
    public void AddItem(ItemData itemAdicionado)
    {
        if (itemAdicionado != null)
        {
            if (itens == null) itens = new List<ItemData>();
            itens.Add(itemAdicionado);
            Debug.Log($"[Inventory] Item adicionado: {itemAdicionado.itemName} foi adicionado ao inventario.");
        }
    }

    // Remove um item do inventario
    public void RemoveItem(ItemData itemRemovido)
    {
        if (itemRemovido != null && itens != null && itens.Contains(itemRemovido))
        {
            itens.Remove(itemRemovido);
            Debug.Log($"[Inventory] Item removido: {itemRemovido.itemName} foi removido do inventario.");
        }
    }
}
