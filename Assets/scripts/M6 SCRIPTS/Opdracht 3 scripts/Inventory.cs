using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Class: verzamelt items
    private List<Item> items = new List<Item>();

    // ScriptableObject: templates laden
    public ItemTemplate[] itemTemplates;

    // Enum: filter op type
    public ItemType filterType;

    void Start()
    {
        // Maak items van templates
        foreach (ItemTemplate template in itemTemplates)
        {
            Item newItem = template.CreateInstance();
            items.Add(newItem);
            Debug.Log(newItem.Describe());
        }
    }

    // Filter items op enum-type
    public List<Item> GetItemsByType(ItemType type)
    {
        List<Item> filtered = new List<Item>();
        foreach (Item item in items)
        {
            if (item.itemType == type) // Enum vergelijking
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Item> filteredItems = GetItemsByType(filterType);
            foreach (Item fi in filteredItems)
            {
                Debug.Log(fi.Describe());
            }

        }
    }
}