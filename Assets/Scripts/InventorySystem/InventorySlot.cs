using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventorySlot : MonoBehaviour
{
    //Inventory UI consists of slots, each slot will have an image, labeltext, and stack size text
    public Image icon;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI stackSizeText;
    
    public void ClearSlot()
    {
        icon.enabled = false;
        labelText.enabled = false;
        stackSizeText.enabled = false;
    }

    // public void DrawSlot(InventoryItem item)
    // {
    //     if (item == null)
    //     {
    //         ClearSlot();
    //         return;
    //     }
    // }
    
    
    
    
    
    
    
    
    
    
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     populateItemsText();
    //     //shows the inventory panel
    //     if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Joystick1Button7)))
    //     {
    //         inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    //     }
    // }
    //
    
}
