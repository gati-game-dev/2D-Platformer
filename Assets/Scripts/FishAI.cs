
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float jumpVelocity = 8f;
    public float waitInterval = 2f;

    public SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite fallingSprite;

    private Rigidbody2D rb;
    private float waterY;
    private bool isJumping = false;
    private float nextJumpTime;

    // Added for player damage
    public Transform player;
    public float damageRange = 1f;
    public int damageAmount = 10;
    public float damageCooldown = 1f;
    private float nextDamageTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waterY = transform.position.y;

        // Keep the fish stationary while waiting
        rb.bodyType = RigidbodyType2D.Static;
        nextJumpTime = Time.time + waitInterval;

        // Find the player if not assigned in the Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        // Start jumping when the wait interval ends
        if (!isJumping && Time.time >= nextJumpTime)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = new Vector2(0f, jumpVelocity);

            isJumping = true;
            spriteRenderer.sprite = upSprite;
        }

        // Reset after falling back into the water
        if (isJumping &&
            transform.position.y <= waterY &&
            rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;

            isJumping = false;
            nextJumpTime = Time.time + waitInterval;
        }

        // Choose sprite based on vertical movement
        if (rb.linearVelocity.y > 0f)
        {
            spriteRenderer.sprite = upSprite;
        }
        else if (rb.linearVelocity.y < 0f)
        {
            spriteRenderer.sprite = fallingSprite;
        }

        // Damage the player with a 1-second cooldown
        if (player != null && Time.time >= nextDamageTime)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= damageRange)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount, transform);
                    nextDamageTime = Time.time + damageCooldown;
                }
            }
        }
    }
}
