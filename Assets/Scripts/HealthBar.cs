using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [HideInInspector]public int playerHealth;
    [SerializeField] private Image[] hearts;
    private Color darkGrey;


    private void Start()
    {
        darkGrey = new Color(0.25f, 0.25f, 0.25f, 1);

    }
    private void Update()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].color = darkGrey;
            }
        }
    }
}
