using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    // Config Parameters
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockParticlesVFX;
    [SerializeField] Sprite[] hitSprites;

    // Cache ref
    Level level;

    // State Variable
    [SerializeField] int timesHit;  // TODO only serialized for debug purposes

    private void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(tag == "Breakable")
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprites();
        }
    }

    private void ShowNextHitSprites()
    {
        int spriteIndex = timesHit - 1;
        if(hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.Log("BLOCK SPRITE MISSING !!!");
        }
    }

    private void DestroyBlock()
    {
        BlockDestroyAudio();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerParticlesVFX();
    }

    private void BlockDestroyAudio()
    {
        FindObjectOfType<GameSession>().AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerParticlesVFX()
    {
        GameObject particles = Instantiate(blockParticlesVFX, transform.position, transform.rotation);
        Destroy(particles, 1f);
    }
}
