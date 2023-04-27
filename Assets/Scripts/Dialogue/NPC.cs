using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool haveMet; //when talking to an NPC for the first time 
    public NPCState state;
    public List<string> linesFirstTimeTalk;
    public List<string> linesQuestLocked;
    public List<string> linesQuestUnlocked;
    public List<string> linesQuestDone;
    public List<string> linesQuestOngoing;
    private List<List<string>> lines = new List<List<string>>(); //2d list that will store all our lines
    public Quest quest;
    public Dialogue dialogue;
    // public int questNumber; <-- use this when quests work well and u want to add more than one
    // quest to npc and each quest correspond to line (lines will be a 2d array) 
    

    private void Start()
    {
        //the line list that has every line
        lines.Add(linesFirstTimeTalk);
        lines.Add(linesQuestLocked);
        lines.Add(linesQuestUnlocked);
        lines.Add(linesQuestDone);
        lines.Add(linesQuestOngoing);
    }


    //when we press E when we're near the NPC we will start a dialogue and set quest to Ongoing.
    private void OnTriggerStay2D(Collider2D target)
    {
        //the npc will have a 2d array of lines, so that when a special event happens, these lines can be later changed
        //we can traverse through the lines using the enum of the quest
        //each value will correspond to a desired array of lines
        //ex: if quest is locked the npc will say smth different than if the quest is ongoing
        //so we will check on the quest status, and depending on that we'll assign dialogue lines


        if (!haveMet)
        {
            //add character name to lines
            dialogue.lines = lines[0];
            if (Input.GetKeyDown(KeyCode.E) && target.gameObject.CompareTag("Player"))
            {
                dialogue.StartDialogue();
                haveMet = true;
            }
        }

        if (haveMet)
        {
            switch (quest.questStatus)
            {
                case QuestStatus.LOCKED:
                    NpcTalk(1, target);
                    break;
                case QuestStatus.UNLOCKED:
                    NpcTalk(2, target);
                    //the quest has started

                    break;
                case QuestStatus.DONE:
                    NpcTalk(3, target);
                    break;
                case QuestStatus.ONGOING:
                    NpcTalk(4, target);
                    break;
            }
        }
    }

    //when we leave make the dialogue empty
    private void OnTriggerExit2D(Collider2D other)
    {
        dialogue.HideCanvas();
        dialogue.lines = new List<string>(); //empty list
    }
    
    private void NpcTalk(int line, Component target)
    {
        //add character name to lines
        dialogue.lines = lines[line];
        if (Input.GetKeyDown(KeyCode.E) && target.gameObject.CompareTag("Player"))
        {
            dialogue.StartDialogue();
        }
    }
}