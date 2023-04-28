using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    //attach this to the enemies
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); //max is exclusive min is inclusive, so roll from 1-100
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
                // return possibleItems;
            }
            
        }

        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)]; //gets only one item randomly
            return droppedItem;
        }
        // Debug.Log("No Loot Dropped");
        return null;
    }

    public void instatiateLoot(Vector2 spawnPoint)
    {
        Loot droppedItem = GetDroppedItem();
        GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPoint, Quaternion.identity);
        lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.itemData.icon; //set the item sprite 
        lootGameObject.GetComponent<ItemCollected>().itemData = droppedItem.itemData; //set the item data, that benfits us when the item is collected
    }
    
}
