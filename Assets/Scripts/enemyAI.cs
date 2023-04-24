using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Transform target;//set target from inspector instead of looking in Update
    public float speed = 3f;
    public Rigidbody2D rb;
    public SwitchWorld mode;
    public int detectionRange = 10;
    [SerializeField] private int health = 100;
    public GameObject BloodEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        float distToPlayer = Vector2.Distance(transform.position, target.position);

        if (distToPlayer < detectionRange && gameObject.tag.Equals("Pixel") && mode.playerMode == SwitchWorld.Mode.VECTOR)
        {
            //code to chase
            ChasePlayer();
        }
        else if(distToPlayer < detectionRange && gameObject.tag.Equals("Vector") && mode.playerMode == SwitchWorld.Mode.PIXEL)
        {
            //code to chase
            ChasePlayer();
        }
        else
        {
            //stop chasing
            StopChasingPlayer();
        }
    }

    public void ChasePlayer()
    {
        if (transform.position.x < target.position.x)
        {
            //follow player to the right
            rb.velocity = new Vector2(speed, 0);
            transform.localScale = new Vector2(1, 1);
        }
        else if(transform.position.x > target.position.x)
        {
            //follow player to the left
            rb.velocity = new Vector2(-speed, 0);
            transform.localScale = new Vector2(-1, 1);
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void StopChasingPlayer()
    {
        rb.velocity = new Vector2(0,0);
    }
    public void TakeDamage(int damage)
    {
        Instantiate(BloodEffect, transform.position, Quaternion.identity);
        health -= damage;
    }
}
