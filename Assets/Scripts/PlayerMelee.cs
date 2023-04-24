using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private float holdVerticalInput;

    //CoolDownTime
    private float TimeBtwAttack;
    public float StartTBA;

    //Hit Range
    public Transform attackPos;
    private Transform attackPosTransform;

    //edit these in editor
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDistance;

    private float groundedAttackRange;
    private float groundedAttackDistance;
    private float jumpAttackRange;
    private float upAttackRange;
    private float downAttackRange;

    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private int damage;
    private PlayerMovement1 PM;

    void Start()
    {
        PM = GetComponent<PlayerMovement1>();
        groundedAttackRange = attackRange;
        groundedAttackDistance = attackDistance;

        jumpAttackRange = attackRange * 1f;
        upAttackRange = attackDistance * 1.1f;
        downAttackRange = attackDistance * 0.6f;

    }

    void Update()
    {
        HitDirection();

        //Debug.Log(TimeBtwAttack);
        if (TimeBtwAttack <= 0)
        {
            if (Input.GetKeyDown("f"))
            {
                Debug.Log("Tried to hit");
                //finds enemies within range and adds them to an array
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<ObjectHealth>().TakeDamage(damage);
                    //Debug.Log(enemiesToDamage.Length);
                }
            }
            TimeBtwAttack = StartTBA;
        }
        else
        {
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}