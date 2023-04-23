using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWorld : MonoBehaviour
{
    public enum Mode
    {
        VECTOR,PIXEL
    }
    public Mode playerMode = Mode.PIXEL;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            if (playerMode == Mode.PIXEL)
            {
                Debug.Log("Player is now in Vector");
                playerMode = Mode.VECTOR;
            }
            else
            {
                Debug.Log("Plyer is now in Pixel");
                playerMode = Mode.PIXEL;
            }
        }
    }
}
