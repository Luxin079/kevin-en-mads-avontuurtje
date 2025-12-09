using UnityEngine;
public class Item
{
    public string itemName;
    public ItemType itemType;
    public ItemStats stats;
    public int sellPrice;
    public bool isEquipped;

    // Constructor
    public Item(string name, ItemType type, ItemStats itemStats, int price)
    {
        itemName = name;
        itemType = type;
        stats = itemStats;
        sellPrice = price;
        isEquipped = false;
    }

    // Methode: geef item-beschrijving
    public string Describe()
    {
        return $"{itemName} ({itemType})\n" +
               $"Damage: {stats.damage}, Defense: {stats.defense}, Weight: {stats.weight}\n" +
               $"Sell Price: {sellPrice}";
    }
}