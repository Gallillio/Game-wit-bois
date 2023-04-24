using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private GameObject BloodEffect;

    void Start()
    {

    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Destroy(Instantiate(BloodEffect, transform.position, Quaternion.identity), 1);
        health -= damage;
    }
}
