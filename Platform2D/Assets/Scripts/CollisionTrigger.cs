using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D platformCollider;
    [SerializeField] private BoxCollider2D platformTrigger;

    private Collider2D[] playerColliders;

    private void Start()
    {
        playerColliders = GameObject.FindGameObjectWithTag("Player").GetComponents<Collider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            for(int i = 0; i < playerColliders.Length; i++)
            {
                Physics2D.IgnoreCollision(platformCollider, playerColliders[i], true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < playerColliders.Length; i++)
            {
                Physics2D.IgnoreCollision(platformCollider, playerColliders[i], false);
            }
        }
    }
}
