using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData itemData;
        public int stackSize; //The Inventory Item consists of the item data(which consists of
                              //the display name, sprite, and the item details)
                              //and the stacksize of how much times do we have.

        public InventoryItem(ItemData item)
        {
            itemData = item;
            AddToStack();
        }

        public void AddToStack()
        {
            stackSize++;
        }

        public void RemoveFromStack()
        {
            stackSize--;
        }
        
    }
}