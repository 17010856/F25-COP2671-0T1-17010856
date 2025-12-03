using UnityEngine;

[CreateAssetMenu(fileName = "NewHarvestableItem", menuName = "Farm/HarvestableItem")]
public class HarvestableItem : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public Sprite itemIcon;
    
    [Header("Value")]
    public int sellPrice = 10;
    
    [TextArea(2, 4)]
    public string description;
}