using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static event Action<List<InventoryItem>> OnInventoryChange;
        public List<InventoryItem> inventory = new(9);

        private Dictionary<ItemData, InventoryItem>
            itemDictionary =
                new(); //if we have an existing item in our inventoru we'll add it to the current stacksize of the item we have, else we'll create a new key value(set to 1) to the item

        [SerializeField] private GameObject inventoryGameObject;
        public CanvasGroup canvasGroup;
        private bool isShowingCanvas; //use this var as a flag to tell whether the canvas is showing or not

        private void Start()
        {
            isShowingCanvas = false;
            HideCanvas();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I) && isShowingCanvas)
                HideCanvas();
            else if (Input.GetKeyDown(KeyCode.I) && !isShowingCanvas) ShowCanvas();
        }

        private void ShowCanvas()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            isShowingCanvas = true;
        }

        private void HideCanvas()
        {
            canvasGroup.alpha = 0f; //this makes everything transparent
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
            isShowingCanvas = false;
        }

        private void OnEnable()
        {
            ItemCollected.OnCollected += Add;
        }


        private void OnDisable()
        {
            ItemCollected.OnCollected -= Add;
        }

        public void Add(ItemData itemData)
        {
            if (itemDictionary.TryGetValue(itemData, out var item))
            {
                //item exists then increment stack size
                item.AddToStack();
                // print("total " + item.itemData.displayName + " " + item.stackSize);
                OnInventoryChange?.Invoke(inventory);
            }
            else if (inventory.Count <= 9)
            {
                //Quick fix
                //if the inventory is full, then do not add to the inventory
                // what i think will happen is the collectible will be deleted 

                //item does not exist so create new entry and add stack size
                var newItem = new InventoryItem(itemData);
                inventory.Add(newItem);
                itemDictionary.Add(itemData, newItem);
                // print("Added " + newItem.itemData.displayName + " " + newItem.stackSize + " for the first time");
                OnInventoryChange?.Invoke(inventory);
            }
        }

        public void Remove(ItemData itemData)
        {
            if (itemDictionary.TryGetValue(itemData, out var item))
            {
                //item exists then decrement stack size
                item.RemoveFromStack();
                if (item.stackSize == 0)
                {
                    inventory.Remove(item);
                    itemDictionary.Remove(itemData);
                }

                OnInventoryChange?.Invoke(inventory);
            }
        }
    }
}