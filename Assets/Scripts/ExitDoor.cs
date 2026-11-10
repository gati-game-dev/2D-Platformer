
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 2f;

    [Header("UI")]
    public GameObject unlockMessage;

    private bool isPlayerInRange = false;
    private bool isUnlocked = false;
    private bool hasPressedE = false;

    public GameManager gameManager;

    void Start()
    {
        unlockMessage.SetActive(false);
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (isUnlocked || hasPressedE)
            return;

        // Check player distance.
        if (player != null)
        {
            float playerDistance = Vector2.Distance(
                player.position,
                transform.position
            );

            isPlayerInRange = playerDistance <= detectionRange;
        }

        // Display unlock message when player is nearby.
        if (isPlayerInRange)
        {
            unlockMessage.SetActive(true);
        }
        else
        {
            unlockMessage.SetActive(false);
        }

        // Check for E key press.
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            hasPressedE = true;

            // Hide unlock message permanently.
            unlockMessage.SetActive(false);

            // Check if player has collected at least one key.
            if (gameManager != null)
            {
                isUnlocked = true;

                gameManager.GameOver();
            }
        }
    }
}
