using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Template")]
public class ItemTemplate : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public ItemStats stats;
    public int basePrice;
    public Sprite icon; // Plaatje voor UI

    // Methode: maak een runtime Item van dit template
    public Item CreateInstance()
    {
        return new Item(itemName, itemType, stats, basePrice);
    }

    // Methode: geef template-info
    public void LogTemplate()
    {
        Debug.Log($"Template: {itemName} (Price: {basePrice})");
    }
}