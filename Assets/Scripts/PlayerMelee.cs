using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// change PM to player after you tell the others
public class PlayerMelee : MonoBehaviour
{
    #region FILE REFERENCES
    private PlayerMovement1 PM;
    //private ObjectHealth objectHealth;
    #endregion

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
    [Header("Damage and KnockBack")]
    [SerializeField] private int damage;
    [SerializeField] private int upwardsKnockback;
    [SerializeField] private int sidewardsKnockback;

    [Header("Settings")]
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

    private float holdVerticalInput; //get when player is holding W or S
    public bool isAttacking = false;

    #region KNOCKBACK EFFECT
    private Vector2 knockbackDirection; //how much the player will go back when hitting an object
    private bool collided; //if player collided with object
    [HideInInspector] public bool downwardStrikeKnockback = false; //if player should go upwards when down attacking an object
    private bool canDownwardStrikeAttack = true; //after downward slicing once in air, you cant do it again till stepping on ground or wall
    #endregion

    Collider2D[] enemiesToDamage;

    int counter = 0;


    private void Start()
    {
        
        anim = GetComponent<Animator>();
        PM = GetComponent<PlayerMovement1>();
        //objectHealth = GetComponent<ObjectHealth>();

        groundedAttackRange = attackRange;
        groundedAttackDistance = attackDistance;

        jumpAttackRange = attackRange * 1f;
        upAttackRange = attackDistance * 1.1f;
        downAttackRange = attackDistance * 0.6f;

        //Vertical Input of Player: -1 = down / 1 = up / 0 = no input
        holdVerticalInput = PM.holdVerticalInput;
    }
    
    private void Update()
    {
        //Resets collision btw enemy and player   
        if (!PM._isDashAttacking)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }

        //Debug.Log(downwardStrikeKnockback);

        if (downwardStrikeKnockback == true)
        {

        }

        HandleMovement();
        HitDirection();

        if (Input.GetKeyDown("f") && !Input.GetButton("Jump"))
        {
            enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
            
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                //if (objectHealth.damagable == true)
                if (enemiesToDamage[i].GetComponent<ObjectHealth>().damagable == true)
                {
                    HandleCollision(enemiesToDamage[i].GetComponent<ObjectHealth>());

                }
            }
        }

        //Debug.Log(TimeBtwAttack);
        //if (TimeBtwAttack <= 0)
        //{
        //    if (Input.GetKeyDown("f"))
        //    {
        //        Debug.Log("Tried to hit");
        //        //finds enemies within range and adds them to an array
        //        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

        //        for (int i = 0; i < enemiesToDamage.Length; i++)
        //        {
        //            //if (objectHealth.damagable == true)
        //            if (enemiesToDamage[i].GetComponent<ObjectHealth>().damagable == true)
        //            {
        //                enemiesToDamage[i].GetComponent<ObjectHealth>().TakeDamage(damage);
        //            }
        //            //Debug.Log(enemiesToDamage.Length);
        //        }
        //    }
        //    TimeBtwAttack = StartTBA;
        //}
        //else
        //{
        //    TimeBtwAttack -= Time.deltaTime;
        //}

    }

    #region ON COLLISION, KNOCKBACK OR NOT
    private void HandleCollision(ObjectHealth objHealth)
    {
        //Debug.Log(isAttacking);

        //if doing downward strick, knockback player upwards
        if (objHealth.giveUpwardForce == true && holdVerticalInput < 0 && !PM.IsGrounded() && canDownwardStrikeAttack)
        {
            knockbackDirection = Vector2.up;
            downwardStrikeKnockback = true;
            collided = true;
            
            //only downwardstrike once until going on ground
            canDownwardStrikeAttack = false;

            //to not change camera if player is attacking
            isAttacking = true;
        }
        //if (PM.IsGrounded() || PM.IsWallSliding)
        //{
        //    isAttacking = false;
        //}

        //if attacking sideways on ground, knockback in opposite direction
        if ((holdVerticalInput <=0 && PM.IsGrounded()) || holdVerticalInput == 0)
        {
            collided = true;
            //is facing right, knockback left
            if (PM.IsFacingRight)
            {
                knockbackDirection = Vector2.left;
            }
            else
            {
                knockbackDirection = Vector2.right;
            }
        }

        objHealth.DealDamage(damage);
        //Start Coroutine, turns off all bools related to melee attack collision and direction
        StartCoroutine(NoLongerColliding());
    }

    private void HandleMovement()
    {

        if (PM.IsOnEnemy())
        {
            float f = PM.RB.transform.position.x;
            PM.RB.transform.position = new Vector3(PM.RB.transform.position.x + 4, PM.RB.transform.position.y, PM.RB.transform.position.z);
        }

        if (collided)
        {
            //if downstrick attack
            if (downwardStrikeKnockback)
            {
                PM.RB.AddForce(knockbackDirection * upwardsKnockback);
            }
            //if side attack
            else
            {
                PM.RB.AddForce(knockbackDirection * sidewardsKnockback);
            }
        }

        // cooldown time test
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

    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSeconds(0.1f);
        collided = false;
        downwardStrikeKnockback = false;
    }

    #endregion
    //changes hitting direction depending on where the player is looking
    private void HitDirection()
    {
        attackPosTransform = transform.Find("AttackPos");

        if (PM.IsWallSliding)
        {
            canDownwardStrikeAttack = true;
        }

        if (PM.IsGrounded())
        {
            attackRange = groundedAttackRange;
            attackDistance = groundedAttackDistance;

            //reset downward strike
            canDownwardStrikeAttack = true;

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