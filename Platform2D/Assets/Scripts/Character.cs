using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [HideInInspector] public bool isAttacking = false;

    [SerializeField] private GameObject attackHitBox;

    protected Rigidbody2D rb2D;
    protected Vector3 velocity = Vector3.zero;
    protected bool facingRight = true;
    protected bool isColliding = false;

    public Animator Animator { get; private set; }

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        attackHitBox.SetActive(false);
    }

    public IEnumerator DoAttack(float activationDelay, float attackDelay)
    {
        Animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(activationDelay);
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(attackDelay);
        Animator.SetBool("IsAttacking", false);
        attackHitBox.SetActive(false);
        isAttacking = false;
    }

    public void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
