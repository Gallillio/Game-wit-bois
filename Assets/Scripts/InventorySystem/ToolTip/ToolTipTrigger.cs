using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //when mouse hovers over object with this script it shows tool tip
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipSystem.Show();
    }
    //when mouse leaves object with this script it hides tool tip
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }
}
