using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using InventorySystem;
using UnityEngine;

public class ItemCollected : MonoBehaviour, ICollectible
{
    public string ItemType;
    public string ItemDetails;
    public static event Action OnCollected; //action that will take place when collected item

    public void Collect()
    {
        Destroy(gameObject); //destroy the item being collected
        // print(ItemType + " has been collected!");
        OnCollected?.Invoke(); //trigger event onCollected
    }
    
    
}
