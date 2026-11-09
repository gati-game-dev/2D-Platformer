
using UnityEngine;
using TMPro;

public class RopeSwinging : MonoBehaviour
{
    public DistanceJoint2D swingJoint;
    public Transform swingAnchor;
    public SpriteRenderer ropeSprite;

    public PlayerMovement playerMovement;

    [Header("Swing Settings")]
    public float swingForce = 10f;
    public float swingDamping = 0.8f;
    public float ropeLength = 3f;
    public float grabRange = 1.5f;

    [Header("Interaction Prompt")]
    public GameObject interactionPromptPrefab;
    public float promptRange = 2f;
    public Vector3 promptOffset = new Vector3(0f, 0.5f, 0f);

    private GameObject promptInstance;
    private TMP_Text promptText;

    private Rigidbody2D rb;
    private float spriteOriginalLength;
    private float originalDamping;

    private SwingRopePoint currentRope;
    private SwingRopePoint promptedRope;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        originalDamping = rb.linearDamping;

        swingJoint.enabled = false;

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Update the interaction prompt.
        UpdateInteractionPrompt();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (swingJoint.enabled)
            {
                // Release the rope and preserve momentum.
                swingJoint.enabled = false;
                playerMovement.isSwinging = false;
                rb.linearDamping = originalDamping;

                if (promptInstance != null)
                {
                    promptInstance.SetActive(false);
                }
            }
            else
            {
                SwingRopePoint nearestRope = FindNearestRope();

                if (nearestRope != null)
                {
                    currentRope = nearestRope;
                    swingAnchor = currentRope.swingAnchor;
                    ropeSprite = currentRope.ropeSprite;

                    if (swingAnchor != null &&
                        ropeSprite != null)
                    {
                        Rigidbody2D anchorBody =
                            swingAnchor.GetComponent<Rigidbody2D>();

                        if (anchorBody != null)
                        {
                            swingJoint.connectedBody = anchorBody;
                            swingJoint.connectedAnchor = Vector2.zero;

                            spriteOriginalLength =
                                ropeSprite.bounds.size.y;

                            // Attach the player.
                            swingJoint.enabled = true;
                            playerMovement.isSwinging = true;

                            rb.linearDamping = swingDamping;

                            // Match the joint to the intended rope length.
                            swingJoint.distance = ropeLength;

                            // Hide the prompt after grabbing.
                            if (promptInstance != null)
                            {
                                promptInstance.SetActive(false);
                            }
                        }
                    }
                }
            }
        }

        // Push once when A/D or arrow keys are pressed.
        if (swingJoint.enabled)
        {
            if (Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.RightArrow))
            {
                rb.AddForce(
                    Vector2.right * swingForce,
                    ForceMode2D.Impulse
                );
            }

            if (Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rb.AddForce(
                    Vector2.left * swingForce,
                    ForceMode2D.Impulse
                );
            }
        }
    }

    void UpdateInteractionPrompt()
    {
        // Do not show the grab prompt while swinging.
        if (swingJoint.enabled)
        {
            if (promptInstance != null)
            {
                promptInstance.SetActive(false);
            }

            promptedRope = null;
            return;
        }

        SwingRopePoint nearestRope = FindNearestRope();

        if (nearestRope == null || nearestRope.swingAnchor == null)
        {
            if (promptInstance != null)
            {
                promptInstance.SetActive(false);
            }

            promptedRope = null;
            return;
        }

        Vector2 ropeBottom =
            (Vector2)nearestRope.swingAnchor.position
            + Vector2.down * ropeLength;

        float distance = Vector2.Distance(
            rb.position,
            ropeBottom
        );

        // Hide the prompt when outside the 2-unit range.
        if (distance > promptRange)
        {
            if (promptInstance != null)
            {
                promptInstance.SetActive(false);
            }

            promptedRope = null;
            return;
        }

        // Create the prompt only once.
        if (promptInstance == null &&
            interactionPromptPrefab != null)
        {
            promptInstance = Instantiate(interactionPromptPrefab);

            promptText =
                promptInstance.GetComponentInChildren<TMP_Text>();

            if (promptText != null)
            {
                promptText.text = "Grab Rope (Press E)";
            }

            promptInstance.SetActive(false);
        }

        if (promptInstance != null)
        {
            promptedRope = nearestRope;

            // Keep the prompt near the rope's bottom.
            promptInstance.transform.position =
                (Vector3)ropeBottom + promptOffset;

            promptInstance.SetActive(true);
        }
    }

    SwingRopePoint FindNearestRope()
    {
        SwingRopePoint[] ropes =
            FindObjectsByType<SwingRopePoint>();

        SwingRopePoint nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (SwingRopePoint rope in ropes)
        {
            if (rope.swingAnchor == null)
                continue;

            Vector2 ropeBottom =
                (Vector2)rope.swingAnchor.position
                + Vector2.down * ropeLength;

            float distance = Vector2.Distance(
                rb.position,
                ropeBottom
            );

            if (distance <= grabRange &&
                distance < nearestDistance)
            {
                nearest = rope;
                nearestDistance = distance;
            }
        }

        return nearest;
    }

    void FixedUpdate()
    {
        if (!swingJoint.enabled)
            return;

        Vector2 anchor = swingAnchor.position;
        Vector2 offset = rb.position - anchor;

        // Restrict the swing to the lower 180-degree arc.
        if (offset.y > 0f)
        {
            float side = offset.x >= 0f ? 1f : -1f;

            rb.position = anchor +
                new Vector2(side * ropeLength, 0f);

            Vector2 velocity = rb.linearVelocity;

            // Prevent the player from moving further
            // outward or above the anchor.
            if (side * velocity.x > 0f)
            {
                velocity.x = 0f;
            }

            if (velocity.y > 0f)
            {
                velocity.y = 0f;
            }

            rb.linearVelocity = velocity;
        }
    }

    void LateUpdate()
    {
        if (currentRope == null ||
            swingAnchor == null ||
            ropeSprite == null)
            return;

        Vector3 start = swingAnchor.position;
        Vector3 end;

        if (swingJoint.enabled)
        {
            end = transform.position;
        }
        else
        {
            end = start + Vector3.down * ropeLength;
        }

        Vector3 direction = end - start;
        float length = direction.magnitude;

        ropeSprite.transform.position = (start + end) / 2f;
        ropeSprite.transform.up = direction.normalized;

        if (spriteOriginalLength <= 0f)
            return;

        Vector3 scale = ropeSprite.transform.localScale;
        scale.y = length / spriteOriginalLength;
        ropeSprite.transform.localScale = scale;
    }

    void OnDestroy()
    {
        if (promptInstance != null)
        {
            Destroy(promptInstance);
        }
    }
}
