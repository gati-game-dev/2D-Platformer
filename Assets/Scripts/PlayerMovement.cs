using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isRopetouched = false;
    public bool isClimbing = false;
    public bool isTouchingSpring = false;
    [Header("Movement")]
    public float speed = 7f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;


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

        else
        {
            rb.linearVelocity = new Vector2(
                horizontalInput * speed,
                rb.linearVelocity.y
            );


            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                float currentJumpForce = jumpForce;

                // Spring gives a stronger jump
                if (isTouchingSpring)
                {
                    currentJumpForce *= 1.5f;
                }

                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    currentJumpForce
                );
            }
        }
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