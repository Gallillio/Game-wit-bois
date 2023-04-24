using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    //CoolDownTime
    private float TimeBtwAttack;
    public float StartTBA;

    //Hit Range
    public Transform attackPos;
    private Transform attackPosTransform;
    private float attackRange = 1f;
    private float groundedAttackRange;
    private float jumpAtackRange;
    private float attackDistance = 0.8f;

    public LayerMask whatIsEnemy;
    public int damage;
    private PlayerMovement1 PM;

    void Start()
    {
        PM = GetComponent<PlayerMovement1>();
        groundedAttackRange = attackRange;
        jumpAtackRange = attackRange * 1.5f;
    }

    void Update()
    {
        HitDirection();
        if (TimeBtwAttack <= 0)
        {
            if (Input.GetKeyDown("f"))
            {
                //Debug.Log("Tried to hit");
                
                //finds enemies within range and adds them to an array
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<enemyAI>().TakeDamage(damage);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

    }

    //changes hitting direction depending on where the player is looking
    private void HitDirection()
    {

        attackPosTransform = transform.Find("attackPos");
        if (PM.IsGrounded()) {
            attackRange = groundedAttackRange;
            if (PM.IsFacingRight)
            {
                attackPosTransform.localPosition = new Vector2(attackDistance, 0);
            }
            else if (!PM.IsFacingRight)
            {
                attackPosTransform.localPosition = new Vector2(attackDistance, 0);
            }
        }
        else
        {
            
            attackPosTransform.localPosition = new Vector2(0, -attackDistance*2f);
            attackRange = jumpAtackRange;
        }

    }


}
