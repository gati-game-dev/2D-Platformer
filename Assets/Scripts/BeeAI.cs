using UnityEngine;

public class BeeAI : MonoBehaviour
{
    public float hoverAmplitude = 0.5f;
    public float hoverSpeed = 1.5f;
    private Vector3 hoverCenter;

    private Transform player;
    public PlayerHealth playerHealth;
    private bool isChasing = false;

    public float chaseSpeed = 3f;

    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    private float nextAttackTime = 0f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;    
    
    void Start()
    {
        hoverCenter = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position + new Vector3(0.5f, 0.5f, 0f), // Adjust the vertical offset as needed
                chaseSpeed * Time.deltaTime
            );

            if (player.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }

                float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                playerHealth.TakeDamage(attackDamage, transform);
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        else
        {
            float verticalOffset =
                Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

            transform.position = new Vector3(
                hoverCenter.x,
                hoverCenter.y + verticalOffset,
                hoverCenter.z
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
            animator.SetBool("IsChasing", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            isChasing = false;
            animator.SetBool("IsChasing", false);
            hoverCenter = transform.position;
        }
    }
}