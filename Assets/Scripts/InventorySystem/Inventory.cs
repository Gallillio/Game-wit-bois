using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory: MonoBehaviour
    {
        public List<InventoryItem> inventory = new List<InventoryItem>();
        private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>(); //if we have an existing item in our inventoru we'll add it to the current stacksize of the item we have, else we'll create a new key value(set to 1) to the item
        public void Add(ItemData itemData)
        {
            if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
            {
                //item exists then increment stack size
                item.AddToStack();
            }
            else
            {
                //item does not exist so create new entry and add stack size
                InventoryItem newItem = new InventoryItem(itemData);
                inventory.Add(newItem);
                itemDictionary.Add(itemData, newItem);
            }
        }
        
        public void Remove(ItemData itemData)
        {
            if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
            {
                //item exists then decrement stack size
                item.RemoveFromStack();
                if (item.stackSize == 0)
                {
                    inventory.Remove(item);
                    itemDictionary.Remove(itemData);
                }
            }
        }
    }
}