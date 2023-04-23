using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Open_Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI itemsText;
    public playerCollectItem collectItem;
    

    //TODO idea, add buttons to check info about the item, and be able to equip the item
    
    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update()
    {
        populateItemsText();
        //shows the inventory panel
        if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Joystick1Button7)))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    void populateItemsText()
    {
        string tmp = "";
        if(collectItem.inventory.Count > 0)
            for (int i = 0; i < collectItem.inventory.Count; i++)
            {
                if(collectItem.inventory[i].Equals("coin"))
                    continue;
                tmp += collectItem.inventory[i] + "\n";
            }
        itemsText.text = tmp;
    }
}
