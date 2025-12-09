using UnityEngine;

public class Weapon : InventoryItem
{
    public Weapon(int quantity = 1) : base("Gun", quantity) {}

    public override void Use()
    {
        Debug.Log("Pew! Pewe! Shooting with gun.");
    }
}