using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Transform target;//set target from inspector instead of looking in Update
    public float speed = 3f;
    public SwitchWorld mode;
 
    void Start () {
         
    }
 
    void Update(){
        //if the target is vector and the enemy is in pixel follow the target 
        if (gameObject.tag.Equals("Pixel") && mode.playerMode == SwitchWorld.Mode.VECTOR)
        {
            //rotate to look at the player
            transform.LookAt(target.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self); //correcting the original rotation


            //move towards the player
            if (Vector3.Distance(transform.position, target.position) > 1f)
            {
                //move if distance from target is greater than 1
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        //if the target is pixel and the enemy is in vector follow the target
        else if (gameObject.tag.Equals("Vector") && mode.playerMode == SwitchWorld.Mode.PIXEL)
        {
            //rotate to look at the player
            transform.LookAt(target.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self); //correcting the original rotation


            //move towards the player
            if (Vector3.Distance(transform.position, target.position) > 1f)
            {
                //move if distance from target is greater than 1
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        
    }
}
