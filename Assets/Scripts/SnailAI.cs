
using UnityEngine;

public class SnailAI : MonoBehaviour
{
    [Header("Movement")]
    public float distance = 5f;
    public float speed = 2f;

    [Header("Player Detection")]
    public Transform player;
    public float scaredDistance = 3f;

    [Header("Animation")]
    public Animator animator;
    public bool isScared = false;

    private float startX;
    private float movementTimer = 0f;

    private SpriteRenderer spriteRenderer;

    private float stoppedX;

    void Start()
    {
        startX = transform.position.x;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        stoppedX = transform.position.x;
    }

    void Update()
    {
        CheckPlayerDistance();

        // Update animation.
        if (animator != null)
        {
            animator.SetBool("isScared", isScared);
        }

        if (isScared)
        {
            StopMovement();
        }
        else
        {
            SnailMovement();
        }
    }

    void CheckPlayerDistance()
    {
        if (player != null)
        {
            float playerDistance = Vector2.Distance(
                player.position,
                transform.position
            );

            isScared = playerDistance <= scaredDistance;
        }
    }

    void StopMovement()
    {
        // Save the exact position where the snail stopped.
        stoppedX = transform.position.x;

        // Keep the snail stationary.
        transform.position = new Vector3(
            stoppedX,
            transform.position.y,
            transform.position.z
        );
    }

    void SnailMovement()
    {
        // Advance timer only while walking.
        movementTimer += Time.deltaTime;

        float x = startX + Mathf.PingPong(
            movementTimer * speed,
            distance * 2f
        ) - distance;

        float previousX = transform.position.x;

        transform.position = new Vector3(
            x,
            transform.position.y,
            transform.position.z
        );

        // Face the direction of movement.
        if (spriteRenderer != null)
        {
            if (x > previousX)
            {
                spriteRenderer.flipX = true;
            }
            else if (x < previousX)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
