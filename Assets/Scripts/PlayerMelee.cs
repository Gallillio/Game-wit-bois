using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    

    //CoolDownTime
    private float TimeBtwAttack;
    public float StartTBA = 0.1f;
    
    private float LastDash;
    //(CoolDown btw each hit while dashing)
    public float DashAttackStrength = 0.04f;

    //Hit Range
    public Transform attackPos;
    private Transform attackPosTransform;

    //edit these in editor
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDistance;

    //float variables used
    private float groundedAttackRange;
    private float groundedAttackDistance;
    private float jumpAttackRange;
    private float upAttackRange;
    private float downAttackRange;
    private float holdVerticalInput;

    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private int damage;

    private PlayerMovement1 PM;
    private Animator anim;



    private void Start()
    {
        
        anim = GetComponent<Animator>();
        PM = GetComponent<PlayerMovement1>();

        groundedAttackRange = attackRange;
        groundedAttackDistance = attackDistance;

        jumpAttackRange = attackRange * 1f;
        upAttackRange = attackDistance * 1.1f;
        downAttackRange = attackDistance * 0.6f;

    }

    private void Update()
    {
        //Resets collision btw enemy and player   
        if (!PM._isDashAttacking)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }

        HitDirection();

        //cooldown time test
        if (TimeBtwAttack <= 0)
        {
            if (PM._isDashAttacking)
            { 
                DashHit();
            }
            else if (Input.GetKeyDown("f"))
            {
                GroundHit();
                TimeBtwAttack = StartTBA;
            }
            
        }
        
        else
        {
            //reset time
            TimeBtwAttack -= Time.deltaTime;
        }
        
    }

    //changes hitting direction depending on where the player is looking
    private void HitDirection()
    {
        attackPosTransform = transform.Find("AttackPos");
        holdVerticalInput = PM.holdVerticalInput;

        if (PM.IsGrounded())
        {
            attackRange = groundedAttackRange;
            attackDistance = groundedAttackDistance;

            //right attack
            if (PM.IsFacingRight)
            {
                attackPosTransform.localPosition = new Vector2(attackDistance, 0);
            }

            //left attack
            else if (!PM.IsFacingRight)
            {
                attackPosTransform.localPosition = new Vector2(attackDistance, 0);
            }

            //up attack on ground
            if (holdVerticalInput > 0)
            {
                attackPosTransform.localPosition = new Vector2(0, attackDistance);
                attackRange = jumpAttackRange;
                attackDistance = upAttackRange;
            }
        }
        else
        {
            holdVerticalInput = PM.holdVerticalInput;

            //down attack in air
            if (holdVerticalInput < 0)
            {
                attackPosTransform.localPosition = new Vector2(0, -attackDistance * 2f);
                attackRange = jumpAttackRange;
                attackDistance = downAttackRange;
            }
            //up attack in air
            else if (holdVerticalInput > 0)
            {
                attackPosTransform.localPosition = new Vector2(0, attackDistance);
                attackRange = jumpAttackRange;
                attackDistance = upAttackRange;
            }
            //side attack in air
            else if (holdVerticalInput == 0)
            {
                attackPosTransform.localPosition = new Vector2(attackDistance, 0);
                attackRange = groundedAttackRange;
                attackDistance = groundedAttackDistance;
            }
        }

        //Dash Attack

        if (PM._isDashAttacking)
        {
            attackPosTransform.localPosition = new Vector2(0, 0);
            attackRange = jumpAttackRange;
        }
    }

    private void GroundHit()
    {
        anim.Play("Sword_Swing");

        //finds enemies within range and adds them to an array
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<ObjectHealth>().TakeDamage(damage);
        }
    }

    private void DashHit()
    {
        if(Time.time - LastDash < DashAttackStrength)
        {

        }
        else
        {
            LastDash = Time.time;
            //disable collision btw enemy and player to go through each other
            Physics2D.IgnoreLayerCollision(3, 7, true);

            //finds enemies within range and adds them to an array
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<ObjectHealth>().TakeDamage(damage);
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}