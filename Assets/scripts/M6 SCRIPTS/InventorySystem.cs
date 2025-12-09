using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    
    int worldMedipacks = 4;
    int worldKeycards = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            PickupItem(new Weapon());

        if (Input.GetKeyDown(KeyCode.M))
            PickupItem(new Medipack());

        if (Input.GetKeyDown(KeyCode.K))
            PickupItem(new Keycard());

        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseItem("Gun");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseItem("Medipack");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            UseItem("Keycard");

        if (Input.GetKeyDown(KeyCode.I))
            DisplayInventory();
    }

    void PickupItem(InventoryItem item)
    {
        if (!CanPickup(item.Name))
        {
            Debug.Log($"No more {item.Name}s in the world to pick up.");
            return;
        }

        var existing = inventory.Find(i => i.Name == item.Name);

        if (existing != null)
            existing.Quantity++;
        else
            inventory.Add(item);

        DecreaseWorld(item.Name);
        Debug.Log($"Picking up a {item.Name}!");
        DisplayWorld();
        DisplayInventory();
    }

    public void UseItem(string itemName)
    {
        var item = inventory.Find(i => i.Name == itemName);

        if (item == null || item.Quantity <= 0)
        {
            Debug.Log($"No {itemName} in inventorye!");
            return;
        }

        item.Use();
        item.Quantity--;

        if (item.Quantity <= 0)
            inventory.Remove(item);

        DisplayInventory();
    }

    void DisplayInventory()
    {
        Debug.Log("=== Inventory ===");
        foreach (var item in inventory)
            Debug.Log($"{item.Name} : {item.Quantity}");
    }

    void DisplayWorld()
    {
        Debug.Log("=== Items in World ===");
        Debug.Log($"medipacks : {worldMedipacks}");
        Debug.Log($"keycards : {worldKeycards}");
    }

    bool CanPickup(string name)
    {
        return name switch
        {
            "Medipack" => worldMedipacks > 0,
            "Keycard" => worldKeycards > 0,
            _ => false
        };
    }

    void DecreaseWorld(string name)
    {
        switch (name)
        {
            case "Medipack": worldMedipacks--; break;
            case "Keycard": worldKeycards--; break;
        }
    }
}
