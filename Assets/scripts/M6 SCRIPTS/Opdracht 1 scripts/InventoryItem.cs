using UnityEngine;

public abstract class InventoryItem
{
    public string Name { get; protected set; }
    public int Quantity { get; set; }

    public InventoryItem(string name, int quantity = 1)
    {
        Name = name;
        Quantity = quantity;
    }

    public abstract void Use();
}