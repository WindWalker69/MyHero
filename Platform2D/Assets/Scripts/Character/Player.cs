using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Character, IKillable<float>
{
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public Stat health;
    public Stat mana;

    public Transform firePoint;
    public Transform groundCheck;
    public Transform ceilingCheck;
    public GameObject magicImpactEffect;
    public LineRenderer lineRenderer;
    public LayerMask whatIsGround;
    public LayerMask magicObstacle;
    public Collider2D crouchDisableCollider;
    public float swordDamage = 35f;
    public float magicDamage = 50f;
    public float runSpeed = 40f;
    public float jumpForce = 400f;
    [Range(0, 1)] public float crouchSpeed = 0.36f;
    [Range(0, 0.3f)] public float movementSmoothing = 0.05f;
    public bool airControl = false;

    private const float groundedRadius = 0.2f;
    private const float ceilingRadius = 0.2f;
    private float groundAttackDelay = 0.4f;
    private float airAttackDelay = 0.2f;
    private float horizontalMove = 0f;
    private bool wasCrouching = false;
    private bool grounded;
    private bool jump;
    private bool crouch;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public BoolEvent OnCrouchEvent;

    protected override void Awake()
    {
        base.Awake();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();

        health.bar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarScript>();
        mana.bar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarScript>();

        health.Initialize();
        mana.Initialize();
    }

    private void Update()
    {
        isColliding = false;

        if (!isAttacking)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            if (grounded)
            {
                Animator.Play("PlayerAttack1");
                StartCoroutine(DoAttack(0f, groundAttackDelay));
            }
            else
            {
                StartCoroutine(DoAttack(0f, airAttackDelay));
            }
            SoundManager.instance.PlayPlayerAttack();
        }

        if (Input.GetButtonDown("Fire2") && !isAttacking && mana.CurrentVal > 0)
        {
            isAttacking = true;
            mana.CurrentVal -= 40f;
            Animator.Play("PlayerCast");
            StartCoroutine(CastMagic());
        }

        if (Input.GetButtonDown("Jump") && !isAttacking)
        {
            jump = true;
            Animator.SetBool("IsJumping", true);
        }

        if (rb2D.velocity.y < -2)
        {
            Animator.SetBool("IsFalling", true);
            Animator.SetBool("IsJumping", false);
        }
        else
            Animator.SetBool("IsFalling", false);

        if (Input.GetButtonDown("Crouch") && !isAttacking)
            crouch = true;
        else if (Input.GetButtonUp("Crouch") && !isAttacking)
            crouch = false;
    }

    public IEnumerator CastMagic()
    {
        yield return new WaitForSeconds(0.4f);

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, magicObstacle);

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeState(new HitState());
                enemy.TakeDamage(magicDamage);
            }

            Instantiate(magicImpactEffect, hitInfo.point, Quaternion.identity);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        SoundManager.instance.PlayCastMagic();

        lineRenderer.enabled = true;

        yield return 0;

        lineRenderer.enabled = false;

        isAttacking = false;
    }

    public void OnLanding()
    {
        Animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        Animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void Move(float move, bool crouch, bool jump)
    {
        Collider2D genericCollider = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround);

        if (!crouch)
            if (genericCollider && !genericCollider.isTrigger)
                crouch = true;

        if (grounded || airControl)
        {
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                move *= crouchSpeed;

                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = false;
            }
            else
            {
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = true;

                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(move * 10f, rb2D.velocity.y);

            if (!isAttacking)
                rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
            else
            {
                if (grounded)
                    rb2D.velocity = Vector3.zero;
                else
                    rb2D.velocity = new Vector3(rb2D.velocity.x * 0.5f, 0f, 0f);
            }

            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }

        if(grounded && jump)
        {
            grounded = false;
            rb2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isColliding)
            return;
        isColliding = true;

        if (other.gameObject.tag == "Weapon" && other.gameObject.layer == 9)
        {
            SoundManager.instance.PlayPlayerHit();
            ScoreManager.instance.RemoveScore(50);
            TakeDamage(other.gameObject.GetComponentInParent<Enemy>().enemyDamage);
        }

        if (other.gameObject.tag == "HealthPotion" && health.CurrentVal < health.MaxVal)
        {
            Destroy(other.gameObject);
            SoundManager.instance.PlayPowerUp();
            health.CurrentVal = health.MaxVal;
        }
        else if (other.gameObject.tag == "HealthPotion" && health.CurrentVal == health.MaxVal)
        {
            StartCoroutine(PlayerManager.instance.FullHealth());
        }

        if (other.gameObject.tag == "ManaPotion" && mana.CurrentVal < mana.MaxVal)
        {
            Destroy(other.gameObject);
            SoundManager.instance.PlayPowerUp();
            mana.CurrentVal = mana.MaxVal;
        }
        else if (other.gameObject.tag == "ManaPotion" && mana.CurrentVal == mana.MaxVal)
        {
            StartCoroutine(PlayerManager.instance.FullMana());
        }

        if (other.gameObject.tag == "DamagePotion")
        {
            Destroy(other.gameObject);
            SoundManager.instance.PlayPowerUp();
            swordDamage *= 2f;
            magicDamage *= 2f;
            StartCoroutine(PlayerManager.instance.DoubleDamage());
        }

        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            SoundManager.instance.PlayGetCoin();
            ScoreManager.instance.AddScore(50);
        }

        if (other.gameObject.tag == "Portal")
        {
            GameManager.instance.YouWin(gameObject);
        }

        if (other.gameObject.name == "DeathLine")
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        health.CurrentVal -= damage;

        if (health.CurrentVal <= 0)
            Die();
    }

    public void Die()
    {
        PlayerManager.instance.RemoveLife();
        ScoreManager.instance.RemoveScore(350);
        GameManager.instance.CheckIfGameOver(gameObject);
    }
}
