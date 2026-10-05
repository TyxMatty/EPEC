using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Scriptable Object/Item")]
public class ItemData : ScriptableObject 
{
    public string itemName;
    [TextArea(2, 4)]
    public string itemDescription;
    public Sprite itemIcon;
}
