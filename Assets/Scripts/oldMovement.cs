using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class oldMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed;
    private float moveInput;
    [HideInInspector] public bool isFacingRight;
    Vector3 rotator;
    
    
    [Header("Jump / Fall")]
    [SerializeField] private int jumpPower;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    [Header("ExtraJump / Cayote - Buffer Time")]
    [SerializeField] private int extraJumpValue;
    private int extraJumps;

    [SerializeField] private float cayoteTime;
    private float cayoteTimeCounter;
    [SerializeField] private float jumpBuffer;
    private float jumpBufferCounter;


    //[Header("Camera Follow")]
    //[SerializeField] private cameraManager cameraManager;
    //[SerializeField] private GameObject cameraFollowPlayer;
    private normalCameraMovement normalCameraMovement;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //normalCameraMovement = cameraFollowPlayer.GetComponent<normalCameraMovement>();
    }

    private void Update()
    {
        JumpSystem();
        //when falling switch to Falling Camera
        //changeCameraFalling();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.94f, 0.16f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private void JumpSystem()
    {

        // cayoteTime
        if (IsGrounded())
        {
            extraJumps = extraJumpValue;
            cayoteTimeCounter = cayoteTime;
        }
        else if (!IsGrounded() && (extraJumps == extraJumpValue - 1 || extraJumps == extraJumpValue))
        {
            cayoteTimeCounter -= Time.deltaTime;
        }
        else
        {
        }
        
        //jumpBuffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        //extraJump True
        if (extraJumps > 0  && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            extraJumps--;

            jumpBufferCounter = 0f;
        }

        //extraJump False AND cayoteTime True
        else if (jumpBufferCounter > 0f && extraJumps == 0 && cayoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpBufferCounter = 0f;
        }

        if (cayoteTimeCounter > 0)
        {
            extraJumps = extraJumpValue;
        }


        //let go of Jump Button
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            cayoteTimeCounter = 0f;
        }
    }

    //private void changeCameraFalling()
    //{
    //    if (rb.velocity.y < -20 && !IsGrounded())
    //    {
    //        //Debug.Log(rb.velocity.y);
    //        cameraManager.SwitchCamera(cameraManager.fallingCamera);
    //    }
    //    else if (rb.velocity.y == 0 || IsGrounded())
    //    {
    //        cameraManager.SwitchCamera(cameraManager.movementCamera);
    //    }
    //    else
    //    {
    //    }
    //}

    private void Movement()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * horizontalSpeed, rb.velocity.y);

        //flips character
        if (!isFacingRight && moveInput < 0)
        {
            Flip();
        }
        else if (isFacingRight && moveInput > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {

        //changes rotation of Y to 180
        if (isFacingRight)
        {
            rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            //turn camera when turn player
            normalCameraMovement.CallTurn();
        }
        else
        {
            rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
            //turn camera when turn player
            normalCameraMovement.CallTurn();
        }



        //changes scaler to -1

        //isFacingRight = !isFacingRight;
        //Vector3 scaler = transform.localScale;

        //scaler.x *= -1;
        //transform.localScale = scaler;
    }


}

