using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

// change PM to player after you tell the others
public class PlayerMelee : MonoBehaviour
{
    #region FILE REFERENCES
    private PlayerMovement1 PM;
    public PlayerData Data;
    private ObjectHealth objectHealth;
    private Rigidbody2D RB;
    private AbilitiesWheel AW;
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
    [SerializeField] private bool disableAbilityCooldown;

    [Header("Player Damaged Knockback")]
    public float knockbackForce;
    public float knockbackCounter;
    public float knockbackTime;
    public bool knockedFromRight;


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

    //[HideInInspector] public bool isDownStriking = false; //if player should go upwards when down attacking an object
    public bool isDownStriking = false; //if player should go upwards when down attacking an object
    public bool canDownwardStrikeAttack = true; //after downward striking once in air, you cant do it again till stepping on ground or wall
    #endregion

    [HideInInspector]public bool IsHitting;
    [HideInInspector] private int dashHitDuration = 0;

    public int Hearts = 4;
    

    private Collider2D[] enemiesToDamage;

    private void Start()
    {
        damage *= 0.5f;
        AW = GetComponent<AbilitiesWheel>();
        PM = GetComponent<PlayerMovement1>();
        RB = GetComponent<Rigidbody2D>();
        
        objectHealth = GetComponent<ObjectHealth>();

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
        HandleMovement();
        HitDirection();
        
        GetComponent<HealthBar>().playerHealth = Hearts;

        //Resets collision btw enemy and player
        if (!PM._isDashAttacking)
        {
                //the ID of layer Player(3) and Enemy(7)
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }

        //only gets knockbacked when not holding space, if holding space he drops again
        if (Input.GetKeyDown("f"))
        {
            IsHitting = true;
            enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                //ggg
                //if (objectHealth.damagable == true)
                    if (enemiesToDamage[i].GetComponent<ObjectHealth>().damagable == true)
                {
                        HandleCollision(enemiesToDamage[i].GetComponent<ObjectHealth>());

                    }
            }
        }
        else
        {
            IsHitting = false;
        }

    }

    #region ON COLLISION, KNOCKBACK OR NOT
    private void HandleCollision(ObjectHealth objHealth)
    {

        //if doing downward strick, knockback player upwards
        if (objHealth.giveUpwardForce == true && holdVerticalInput < 0 && !PM.IsGrounded() && canDownwardStrikeAttack)
        {
            knockbackDirection = Vector2.up;
            isDownStriking = true;
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

        //ggg
        objHealth.DealDamage(damage);
        //Start Coroutine, turns off all bools related to melee attack collision and direction
        StartCoroutine(NoLongerColliding());
    }

    public void HandleMovement()
    {
        //Resets collision btw enemy and player   
        if (!PM._isDashAttacking)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }
        if (PM.IsGrounded() || PM.IsWallSliding)
        {
            isDownStriking = false;
        }
        if (collided)
        {
            //if downstrick attack
            if (isDownStriking)
            {
                //PM.SetGravityScale(0);
                PM.RB.velocity = new Vector2(PM.RB.velocity.x, upwardsKnockback);
            }
            //if side attack
            else
            {
                //use this later to fix holding the button from cancelling knockback
                //PM.RB.velocity = new Vector2(sidewardsKnockback, PM.RB.velocity.y);
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
                IsHitting = true;
                TimeBtwAttack = StartTBA;
            }
            else
            {
                IsHitting = false;
            }
        }

        else
        {
            //reset time
            TimeBtwAttack -= Time.deltaTime;
        }
    }

    public IEnumerator NoLongerColliding()
    {
        //yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(0.3f);
        collided = false;
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

        holdVerticalInput = PM.holdVerticalInput;
        if (PM.IsGrounded())
        {
            attackRange = groundedAttackRange;
            attackDistance = groundedAttackDistance;

            //reset downward strike
            canDownwardStrikeAttack = true;

            //right attack

            //down attack in air
            //up attack in air
            if (holdVerticalInput > 0)
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
        else
        {
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
        //Check if desired ability is chosen & cooldown time finished
        if (AW.abilitiesChoice == 0 && AW.cooldownDuration == 0 || disableAbilityCooldown)
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
                //Bug fix: this part replays 7 times per dash for some reason sooo
                if(dashHitDuration == 7 && !disableAbilityCooldown)
                {
                    //start cooldown timer
                    AW.cooldownDuration = 12;
                    AW.BeginWheelCountdown(AW.cooldownDuration);
                    dashHitDuration = 0;
                }
                else
                {
                    dashHitDuration++;
                }
            }
        }
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (PM._isDashAttacking && collision.gameObject.tag == "BreakableWall")
        {
                Destroy(collision.gameObject);
            }
            else
            {

            }

    }


    public void PlayerDamaged()
    {
        GetComponent<HealthBar>().playerHealth -= 1;
        Hearts -= 1;
        PlayerKnockbackFromHit();
    }

    public void PlayerKnockbackFromHit()
    {
        if (knockbackCounter <= 0)
        {
            
        }
        else
        {
            if (knockedFromRight)
            {
                RB.velocity = new Vector2(-knockbackForce*100, 0);
            }
            else
            {
                RB.velocity = new Vector2(knockbackForce*100, 0);
            }

            knockbackCounter -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}