using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] private Transform target; //set target from inspector instead of looking in Update
    private float speed = 3f;
    private Rigidbody2D rb;
    public GameObject player;
    [SerializeField] private SwitchWorld mode;
    [SerializeField] private int detectionRange = 10;
    private float lastHit;
    public float enemyHitCoolDown = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, target.position);

        if (distToPlayer < detectionRange && gameObject.tag.Equals("Pixel") && mode.currentPlayerMode == SwitchWorld.Mode.VECTOR)
        {
            //code to chase
            ChasePlayer();
        }
        else if(distToPlayer < detectionRange && gameObject.tag.Equals("Vector") && mode.currentPlayerMode == SwitchWorld.Mode.PIXEL)
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


    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            if (Time.time - lastHit < enemyHitCoolDown)
            {
            }
            else
            {
                lastHit = Time.time;
                //ggg
                collision.gameObject.GetComponent<PlayerMelee>().PlayerDamaged();

            }

        }
    }
}
