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
        // else
        // {
        //     GetDroppedItem(); //call the function again if nothing was dropped
        // }
        Debug.Log("No Loot Dropped");
        return null;
    }

    public void instatiateLoot(Vector2 spwanPoint)
    {
        Loot droppedItem = GetDroppedItem();
        GameObject lootGameObject = Instantiate(droppedItemPrefab, spwanPoint, Quaternion.identity);
        lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite; //edit this later to accept items
        
    }
    
}
