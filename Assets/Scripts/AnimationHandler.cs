using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private PlayerMelee PML;
    private PlayerMovement1 PM;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        PM = GetComponent<PlayerMovement1>();
        PML = GetComponent<PlayerMelee>();
    }

    void Update()
    {
        if (PM.IsJumping)
        {
            anim.Play("Player_Jump");
        }
        if (PM.IsDashing)
        {
            anim.SetBool("Dashing", true);
        }
        else
        {
            anim.SetBool("Dashing", false);
        }
        if (PM._moveInput.x != 0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
        if (PML.IsHitting)
        {
            anim.Play("Player_Hit");
        }
    }
}