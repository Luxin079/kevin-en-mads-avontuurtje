using UnityEngine;

public class Medipack : InventoryItem
{
    public Medipack(int quantity = 1) : base("Medipack", quantity) {}

    public override void Use()
    {
        Debug.Log("Healing with medipacck.");
    }
}