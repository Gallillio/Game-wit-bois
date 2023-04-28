using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    //create the basic outline for a loot (i think we can use items)
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;

    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
