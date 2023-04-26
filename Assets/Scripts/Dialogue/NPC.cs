using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum NPCState
{
    AGRESSIVE,
    PASSIVE,
    DEAD,
    ALIVE
}


public class NPC : MonoBehaviour
{
    public string name;
    public string[] lines; 
    public Quest quest;
    public Dialogue dialogue;

    private void Start()
    {
        name = "test";
        quest = new Quest();
    }

    //when we press E when we're near the NPC we will start a dialogue and set quest to Ongoing.
    private void OnTriggerStay2D(Collider2D other)
    {
        dialogue.lines = lines;
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.CompareTag("Player"))
        {
            print("NPC HELLO");
            dialogue.StartDialogue();
        }
    }
}
