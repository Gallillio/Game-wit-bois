using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Open_Inventory : MonoBehaviour
{
    public GameObject panel;
    public TextMesh items;
    public GameObject player;
    
    // Update is called once per frame
    void Update()
    {
        
        
        if (!panel.activeSelf && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Joystick1Button7)))
        {
            panel.SetActive(!panel.activeSelf);
            populateItemsText();
        }
    }

    void populateItemsText()
    {
        
    }
}
