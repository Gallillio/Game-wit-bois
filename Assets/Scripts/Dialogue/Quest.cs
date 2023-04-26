using System;
using UnityEngine;

public enum QuestType
{
    NONE,
    FETCH,
    BOUNTY_HUNT,
    OTHER
}

public enum QuestStatus
{
    LOCKED,
    UNLOCKED,
    DONE,
    ONGOING
}

public class Quest: ScriptableObject
{
   [SerializeField] public string questName;
   [SerializeField] public QuestType questType;
   [SerializeField] public QuestStatus questStatus;
    
   
}