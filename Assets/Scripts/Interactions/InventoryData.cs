using System.Collections.Generic;
using UnityEngine;


// ScriptableObject que armazena a lista de itens atualmente no inventario do Player
// Como no GlobalStateManager, itens pegos em Play Mode ficam no asset; o SaveManager chama Limpar() quando nao ha save
// REVIEW: os READMEs citam AdicionarItem/RemoverItem; os metodos reais sao Additem/AddItem/RemoveItem.
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

    // true se a mochila tem um item com esse itemName ou itemID (usado pelas Condicoes de item)
    public bool ContemItem(string nomeOuId)
    {
        return itens != null && itens.Exists(i => i != null && (i.itemName == nomeOuId || i.itemID == nomeOuId));
    }

    // Esvazia a mochila (Novo Jogo e antes de carregar um save)
    public void Limpar()
    {
        itens = new List<ItemData>();
    }
}
