using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Character, IKillable<float>
{
    [SerializeField] private float health = 100;

    public float enemyAttackDelay = 0.7f;
    public float activationDelay = 0.6f;
    public float enemyDamage = 10f;
    public GameObject deathEffect;
    public Transform target;
    public float speed = 250f;
    public float updatePathRate = 2f;
    public float nextWayPointDistance = 1f;

    private Path path;
    private Seeker seeker;
    private IEnemyState currentState;
    private int currentWayPoint = 0;
    private float rejectForce = 100f;
    private bool reachedEndOfPath = false;

    protected override void Awake()
    {
        base.Awake();
        target = GameManager.instance.FindPlayer().transform;
        seeker = GetComponent<Seeker>();
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
        StartCoroutine(UpdatePath());
    }

    private IEnumerator UpdatePath()
    {
        if (target == null)
        {
            yield return new WaitForSeconds(GameManager.instance.playerRespawnDelay);
            target = GameManager.instance.FindPlayer().transform;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updatePathRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void Update()
    {
        isColliding = false;

        currentState.Execute();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        Animator.SetBool("IsIdling", false);

        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (reachedEndOfPath)
                return;
            reachedEndOfPath = true;
        }
        else
            reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb2D.AddForce(force);
        
        float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        if (rb2D.velocity.x >= 0.01f && !facingRight)
            Flip();
        else if (rb2D.velocity.x <= -0.01f && facingRight)
            Flip();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon" && other.gameObject.layer == 8)
        {
            ChangeState(new HitState());
        }
        currentState.OnTriggerEnter(other);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (isColliding)
            return;
        isColliding = true;

        if (other.gameObject.tag == "Player")
        {
            rb2D.velocity = Vector3.zero;
            if (other.gameObject.transform.position.x < transform.position.x)
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-rejectForce, other.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {                
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rejectForce, other.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            other.gameObject.GetComponent<Player>().TakeDamage(enemyDamage);
            SoundManager.instance.PlayPlayerHit();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        ScoreManager.instance.AddScore(150);
        Destroy(gameObject);
    }
}
