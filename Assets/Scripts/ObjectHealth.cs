using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    public bool damagable = true; //if the object will die or not
    public bool giveUpwardForce = true; // can the player bounce on it
    [SerializeField] private float health; //how many hits before dying
    [SerializeField] private GameObject BloodEffect; //animation for after dying

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

    public void DealDamage(float damage)
    {
        Destroy(Instantiate(BloodEffect, transform.position, Quaternion.identity), 1);
        health -= damage;
    }
}
