using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField] private Collider2D[] others;

    private void Awake()
    {
        for (int i = 0; i < others.Length; i++)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), others[i], true);
        }
    }
}
