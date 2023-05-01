using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    public bool damagable = true; //if the object will die or not
    public bool giveUpwardForce = true; // can the player bounce on it
    [SerializeField] private float health; //how many hits before dying
    [SerializeField] private GameObject BloodEffect; //animation for after dying
    [SerializeField] private AudioSource enemySfxAudioSource;
    public List<AudioClip> DamageSfx;
    
    void Start()
    {

    }

    void Update()
    {
        if (health <= 0)
        {
            if (gameObject.tag.Equals("Pixel") || gameObject.tag.Equals("Vector"))
            {
                gameObject.GetComponent<LootBag>().instatiateLoot(transform.position);
            }
            Destroy(gameObject);
        }
    }

    public void DealDamage(float damage)
    {
        Destroy(Instantiate(BloodEffect, transform.position, Quaternion.identity), 1);
        health -= damage;
        enemySfxAudioSource.clip = DamageSfx[Random.Range(0,DamageSfx.Count)]; //plays random sound from the list
        enemySfxAudioSource.Play();
    }
}
