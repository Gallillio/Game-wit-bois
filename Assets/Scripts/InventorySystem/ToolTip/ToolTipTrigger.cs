using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public InventorySlot item;
    
    //when mouse hovers over object with this script it shows tool tip
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        string content = item.itemDetails;
        string header = item.displayName;
        ToolTipSystem.Show(content,header); //send name to the system and the system will display the name and details
    }
    //when mouse leaves object with this script it hides tool tip
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }
}
