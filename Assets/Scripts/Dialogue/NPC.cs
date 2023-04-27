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
        //add character name to lines
        dialogue.lines = lines;
        if (Input.GetKeyDown(KeyCode.E) && target.gameObject.CompareTag("Player"))
        {
            dialogue.StartDialogue();
        }
    }
    
    //when we leave make the dialogue empty
    private void OnTriggerExit2D(Collider2D other)
    {
        dialogue.HideCanvas();
        dialogue.lines = Array.Empty<string>();
    }
}
