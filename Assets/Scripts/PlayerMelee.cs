using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// change PM to player after you tell the others
public class PlayerMelee : MonoBehaviour
{
    #region FILE REFERENCES
    private PlayerMovement1 PM;
    public PlayerData Data;
    //private ObjectHealth objectHealth;
    #endregion
    #region SETTERS
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
    [SerializeField] private float damage;
    [SerializeField] private float upwardsKnockback;
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

    [SerializeField] private LayerMask whatIsEnemy;

    private float holdVerticalInput; //get when player is holding W or S
    #endregion
    #region KNOCKBACK EFFECT
    private Vector2 knockbackDirection; //how much the player will go back when hitting an object
    private bool collided; //if player collided with object

    [HideInInspector] public bool downwardStrikeKnockback = false; //if player should go upwards when down attacking an object
    public bool canDownwardStrikeAttack = true; //after downward striking once in air, you cant do it again till stepping on ground or wall
    #endregion
    public bool IsHitting;

    private Collider2D[] enemiesToDamage;

    
    private void Start()
    {

        damage *= 0.5f;
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
        //print(Data.jumpForce);

        //Resets collision btw enemy and player   
        if (!PM._isDashAttacking)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }

        if (downwardStrikeKnockback == true)
        {
            //Debug.Log("aa");
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

    }

    #region ON COLLISION, KNOCKBACK OR NOT
    private void HandleCollision(ObjectHealth objHealth)
    {

        //if doing downward strick, knockback player upwards
        if (objHealth.giveUpwardForce == true && holdVerticalInput < 0 && !PM.IsGrounded() && canDownwardStrikeAttack)
        {
            knockbackDirection = Vector2.up;
            downwardStrikeKnockback = true;
            collided = true;

            //only downwardstrike once until going on ground
            canDownwardStrikeAttack = false;

        }


        //if attacking sideways on ground, knockback in opposite direction
        if ((holdVerticalInput <= 0 && PM.IsGrounded()) || holdVerticalInput == 0)
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
                ////Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
                //float gravityStrength = -(2 * upwardsKnockback) / (Data.jumpTimeToApex * Data.jumpTimeToApex);

                ////Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
                //float gravityScale = gravityStrength / Physics2D.gravity.y;

                //////Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
                //float force = Mathf.Abs(Data.gravityStrength) * Data.jumpTimeToApex;

                //if (PM.RB.velocity.y < 0)
                //    force -= PM.RB.velocity.y;
                //PM.RB.AddForce(knockbackDirection * force, ForceMode2D.Impulse);

                //PM.Jump();
                PM.RB.AddForce(knockbackDirection * upwardsKnockback, ForceMode2D.Impulse);
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
        attackPosTransform = transform.Find("AttackPosition");

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


        //finds enemies within range and adds them to an array
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<ObjectHealth>().DealDamage(damage);
        }
    }

    private void DashHit()
    {
        if (Time.time - LastDash < DashAttackStrength)
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
                enemiesToDamage[i].GetComponent<ObjectHealth>().DealDamage(damage);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}