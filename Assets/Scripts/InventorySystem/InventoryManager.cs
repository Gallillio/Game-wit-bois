using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;
    //we have 9 possible slots, so the list will be of size 9. Meaning 9 different items
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(9);
    public GameObject inventoryTitleText;

    private void OnEnable()
    {
        Inventory.OnInventoryChange += DrawInventory;
    }
    
    private void OnDisable()
    {
        Inventory.OnInventoryChange -= DrawInventory;
    }
    
    void ResetInventroy()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy((childTransform.gameObject));
        }
        inventorySlots = new List<InventorySlot>(9);
    }

    void DrawInventory(List<InventoryItem> inventory)
    {
        ResetInventroy();

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            CreateInventorySlot();
        }

        for (int i = 0; i < inventory.Capacity; i++)
        {
            inventorySlots[i].DrawSlot(inventory[i]);
        }
        
    }
    void CreateInventorySlot()
    {
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.SetParent(transform, false);

        InventorySlot newSlotsComponent = newSlot.GetComponent<InventorySlot>();
        newSlotsComponent.ClearSlot();
        
        inventorySlots.Add(newSlotsComponent);
    }
}
