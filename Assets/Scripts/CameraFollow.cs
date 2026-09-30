using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D playerRb;

    public float horizontalDeadZone = 3f;
    public float verticalDeadZone = 3f;

    public float verticalBias = -1.5f;
    public float lookAheadX = 2f;

    public float smoothTime = 0.4f;
    public float lookAheadSmoothTime = 0.3f;

    private Vector3 cameraVelocity = Vector3.zero;
    private float currentLookAhead = 0f;
    private float lookAheadVelocity = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // -------------------------
        // HORIZONTAL DEAD ZONE
        // -------------------------

        if (player.transform.position.x >
            transform.position.x + horizontalDeadZone)
        {
            targetX = player.transform.position.x - horizontalDeadZone;
        }

        if (player.transform.position.x <
            transform.position.x - horizontalDeadZone)
        {
            targetX = player.transform.position.x + horizontalDeadZone;
        }


        // -------------------------
        // SMOOTH HORIZONTAL LOOK-AHEAD
        // -------------------------

        float desiredLookAhead = 0f;

        if (playerRb.linearVelocity.x > 0.1f)
        {
            desiredLookAhead = lookAheadX;
        }
        else if (playerRb.linearVelocity.x < -0.1f)
        {
            desiredLookAhead = -lookAheadX;
        }

        currentLookAhead = Mathf.SmoothDamp(
            currentLookAhead,
            desiredLookAhead,
            ref lookAheadVelocity,
            lookAheadSmoothTime
        );

        targetX += currentLookAhead;


        // -------------------------
        // VERTICAL DEAD ZONE
        // -------------------------

        if (player.transform.position.y >
            transform.position.y + verticalDeadZone - verticalBias)
        {
            targetY = player.transform.position.y
                - verticalDeadZone
                + verticalBias;
        }

        if (player.transform.position.y <
            transform.position.y - verticalDeadZone - verticalBias)
        {
            targetY = player.transform.position.y
                + verticalDeadZone
                + verticalBias;
        }


        // -------------------------
        // MOVE CAMERA
        // -------------------------

        Vector3 targetPosition = new Vector3(
            targetX,
            targetY,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref cameraVelocity,
            smoothTime
        );
    }
}