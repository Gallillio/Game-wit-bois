using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchWorld : MonoBehaviour
{
    public enum Mode
    {
        VECTOR,PIXEL
    }
    [FormerlySerializedAs("playerMode")] public Mode currentPlayerMode = Mode.PIXEL;
    [SerializeField] private AudioSource switchModeSfxAudioSource;
    public AudioClip switchSfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //switch player from current mode to next mode
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            if (currentPlayerMode == Mode.PIXEL)
            {
                Debug.Log("Player is now in Vector");
                currentPlayerMode = Mode.VECTOR;
            }
            else
            {
                Debug.Log("Plyer is now in Pixel");
                currentPlayerMode = Mode.PIXEL;
            }

            switchModeSfxAudioSource.clip = switchSfx;
            switchModeSfxAudioSource.Play();
        }
    }
}
