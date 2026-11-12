
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
    public float maxSwingSpeed = 8f;
    public float maxReleaseSpeed = 8f;

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
                // Release the rope while limiting launch velocity.
                rb.linearVelocity = Vector2.ClampMagnitude(
                    rb.linearVelocity,
                    maxReleaseSpeed
                );

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

        // Restrict the swing to a 150-degree arc (75 degrees on each side).
        float maxAngle = 75f;
        float currentAngle = Mathf.Atan2(offset.x, -offset.y) * Mathf.Rad2Deg;

        if (Mathf.Abs(currentAngle) > maxAngle)
        {
            float clampedAngle = Mathf.Sign(currentAngle) * maxAngle;

            Vector2 clampedOffset = new Vector2(
                Mathf.Sin(clampedAngle * Mathf.Deg2Rad),
                -Mathf.Cos(clampedAngle * Mathf.Deg2Rad)
            ) * ropeLength;

            rb.position = anchor + clampedOffset;

            Vector2 velocity = rb.linearVelocity;

            Vector2 outwardDirection = new Vector2(
                Mathf.Sign(clampedAngle) * Mathf.Cos(clampedAngle * Mathf.Deg2Rad),
                Mathf.Sin(clampedAngle * Mathf.Deg2Rad)
            );

            float outwardVelocity = Vector2.Dot(
                velocity,
                outwardDirection
            );

            if (outwardVelocity > 0f)
            {
                velocity -= outwardVelocity * outwardDirection;
            }

            rb.linearVelocity = velocity;
        }

        // Apply controlled swinging force.
        if (Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(
                Vector2.right * swingForce,
                ForceMode2D.Impulse
            );
        }

        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(
                Vector2.left * swingForce,
                ForceMode2D.Impulse
            );
        }

        // Prevent excessive velocity while swinging.
        rb.linearVelocity = Vector2.ClampMagnitude(
            rb.linearVelocity,
            maxSwingSpeed
        );
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
