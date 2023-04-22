using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class playerCollectItem : MonoBehaviour
{

    private List<string> inventory;

    private int coinCount;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        inventory = new List<string>();
    }

    void Update()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            string itemType = collision.gameObject.GetComponent<itemCollected>().ItemType;

            inventory.Add(itemType);

            if (itemType == "coin")
            {
                coinCount++;
                print("Coins: " + coinCount);
            }


            //prints what is in list
            for (int i = 0; i < inventory.Count; i++)
            {
                print("Item: " + inventory[i]);
            }
        }

        Destroy(collision.gameObject);
    }
}
