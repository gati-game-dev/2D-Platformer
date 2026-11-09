
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Detection")]
    public Transform player;
    public Animator animator;
    public float triggerZone = 5f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float jumpForce = 4f;
    public float jumpInterval = 1.5f;
    public bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Damage")]
    public int damageAmount = 25;
    public float damageCooldown = 1f;

    private Rigidbody2D rb;
    private bool isAwakened = false;
    private float nextJumpTime;
    private float nextDamageTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Boss starts at rest.
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        if (animator != null)
        {
            animator.SetBool("isAwakened", false);
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // Detect whether the boss is touching the ground.
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Awaken when the player enters the trigger zone.
        if (!isAwakened)
        {
            float distanceToPlayer = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distanceToPlayer <= triggerZone)
            {
                AwakenBoss();
            }
        }

        if (!isAwakened)
            return;

        // Follow the player's X direction only.
        float horizontalDifference =
            player.position.x - new Vector2(0.7f, 0f).x - transform.position.x;

        float direction = 0f;

        if (horizontalDifference > 0.15f)
        {
            direction = 1f;
        }
        else if (horizontalDifference < -0.15f)
        {
            direction = -1f;
        }

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );

            // Jump periodically, but only when grounded.
            if (isGrounded && Time.time >= nextJumpTime)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                isGrounded = false;
                nextJumpTime = Time.time + jumpInterval;
            }
    }

    void AwakenBoss()
    {
        isAwakened = true;

        // Enable physics so the boss can move and jump.
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        nextJumpTime = Time.time + jumpInterval;

        if (animator != null)
        {
            animator.SetBool("isAwakened", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage the player on physical collision.
        if (collision.gameObject.CompareTag("Player") &&
            Time.time >= nextDamageTime)
        {
            PlayerHealth playerHealth =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount, transform);
                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Allow repeated damage after the cooldown while touching player.
        if (collision.gameObject.CompareTag("Player") &&
            Time.time >= nextDamageTime)
        {
            PlayerHealth playerHealth =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}
