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
    public NPCState state;
    public string[] lines;
    // public int questNumber; <-- use this when quests work well and u want to add more than one
    // quest to npc and each quest corsponds to line (lines will be a 2d array) 
    public Quest quest;
    public Dialogue dialogue;

    //when we press E when we're near the NPC we will start a dialogue and set quest to Ongoing.
    private void OnTriggerStay2D(Collider2D target)
    {
        dialogue.lines = lines;
        print("player close");
        if (Input.GetKeyDown(KeyCode.E) && target.gameObject.CompareTag("Player"))
        {
            print(target.tag);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            print("NPC HELLO");
            dialogue.StartDialogue();
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
