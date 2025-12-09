using UnityEngine;

public class Keycard : InventoryItem
{
    public Keycard(int quantity = 1) : base("Keycard", quantity) {}

    public override void Use()
    {
        Debug.Log("Swiping keycard to open a door.");
    }
}