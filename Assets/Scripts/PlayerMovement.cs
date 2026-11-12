using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isRopetouched = false;
    public bool isClimbing = false;
    public bool isTouchingSpring = false;
    public bool isHit = false;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float knockbackUpwardForce = 0.5f;

    [Header("Movement")]
    public float speed = 7f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    public bool isSwinging = false;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // Check if the player is standing on the ground
        CheckGround();

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        animator.SetBool("IsJumping", !isGrounded);

        // Stop normal movement while taking damage
        if (isHit)
        {
            return;
        }

        // -------------------------
        // START CLIMBING
        // -------------------------

        if (isRopetouched && verticalInput != 0)
        {
            isClimbing = true;
        }


        // -------------------------
        // LEAVE ROPE
        // -------------------------

        if (isClimbing && (Input.GetKeyDown(KeyCode.Space) || horizontalInput != 0))
        {
            isClimbing = false;
        }


        // -------------------------
        // CLIMBING MOVEMENT
        // -------------------------

        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(0, verticalInput * speed);
        }


        // -------------------------
        // NORMAL MOVEMENT
        // -------------------------

        else if (!isSwinging)
        {
            rb.linearVelocity = new Vector2(
                horizontalInput * speed,
                rb.linearVelocity.y
            );

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                float currentJumpForce = jumpForce;

                if (isTouchingSpring)
                {
                    currentJumpForce *= 1.8f;
                }

                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    currentJumpForce
                );
            }
        }
    }
    public void StartHit()
    {
        isHit = true;
        animator.SetTrigger("Hit");
    }
    public void FinishHit()
    {
        isHit = false;
    }

    // -------------------------
    // GROUND CHECK
    // -------------------------

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
    public void ApplyKnockback(Transform damageSource)
    {
        Vector2 direction;

        if (damageSource != null)
        {
            // Calculate direction away from the damage source
            float horizontalDirection = Mathf.Sign(
                transform.position.x - damageSource.position.x
            );

            direction = new Vector2(
                horizontalDirection,
                knockbackUpwardForce
            ).normalized;
        }
        else
        {
            // Environmental damage without a known source:
            // apply an upward knockback
            direction = Vector2.up;
        }

        // Reset existing velocity
        rb.linearVelocity = Vector2.zero;

        // Apply immediate knockback
        rb.AddForce(
            direction * knockbackForce,
            ForceMode2D.Impulse
        );
    }

    // Shows the ground-check circle in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}