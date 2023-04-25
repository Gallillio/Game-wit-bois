using System;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using TMPro;


public class PlayerCollectItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollectible collectible = other.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.Collect();
        }
    }
}
