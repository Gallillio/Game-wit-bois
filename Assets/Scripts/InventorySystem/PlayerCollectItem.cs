using System;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class PlayerCollectItem : MonoBehaviour
{
    [SerializeField] private AudioSource collectItemSfxAudioSource;
    public List<AudioClip> collectSfx;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectItemSfxAudioSource.clip = collectSfx[Random.Range(0,collectSfx.Count)]; //plays random sound from the list
            collectItemSfxAudioSource.Play();
            collectible.Collect();
        }
    }
}