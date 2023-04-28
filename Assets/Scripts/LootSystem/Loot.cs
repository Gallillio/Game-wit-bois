using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    //create the basic outline for a loot (i think we can use items)
    public int dropChance; //chance of the item dropping
    public ItemData itemData; //refrence of the item data

    public Loot(int dropChance, ItemData itemData) //constructor of the object
    {
        this.dropChance = dropChance;
        this.itemData = itemData;
    }
}
