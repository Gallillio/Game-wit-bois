using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public SwitchWorld playerMode;
    //each obstacle will have this script, and based on their tag, they'll hurt the player or do nothing to the player
    
    private void OnCollisionStay2D(Collision2D target)
    {
        //if this gameobject and the target are the same type, then you good bro, else you're damaged
        // todo damamge player health at the else statement block 
        if (gameObject.CompareTag(target.gameObject.tag) || (target.gameObject.tag.Equals("Player") && playerMode.currentPlayerMode.ToString().Equals(gameObject.tag.ToUpper())))
        {
            print("you good bro");
        }
        else
        {
            print("pow damage");
        }
    }
}
