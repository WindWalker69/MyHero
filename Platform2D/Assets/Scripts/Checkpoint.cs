using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer checkpointSprite;

    private void Awake()
    {
        checkpointSprite = GetComponent<SpriteRenderer>();
        checkpointSprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            checkpointSprite.enabled = true;
            GameManager.instance.lastCheckpointPos = transform;
        }
    }
}
