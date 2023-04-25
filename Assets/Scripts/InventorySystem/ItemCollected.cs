using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using InventorySystem;
using TMPro;
using UnityEngine;

public class ItemCollected : MonoBehaviour, ICollectible
{
    public string ItemType;
    public static event HandleItemCollected OnCollected; //action that will take place when collected item
    public delegate void HandleItemCollected(ItemData itemData);

    public ItemData itemData;

    public void Collect()
    {
        Destroy(gameObject); //destroy the item being collected
        // print(ItemType + " has been collected!");
        OnCollected?.Invoke(itemData); //trigger event onCollected
    }
    
    
}
