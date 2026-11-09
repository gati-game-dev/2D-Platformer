
using UnityEngine;
using TMPro;

public class LeverPulled : MonoBehaviour
{
    public Transform player;

    private bool isLeverPulled = false;
    public bool IsLeverPulled { get { return isLeverPulled; } }

    private SpriteRenderer leverSpriteRenderer;
    public Sprite pulledLeverSprite;

    [Header("Interaction Prompt")]
    public GameObject interactionPromptPrefab;
    public float promptRange = 2f;
    public Vector3 promptOffset = new Vector3(0f, 1.2f, 0f);

    private GameObject promptInstance;
    private TMP_Text promptText;

    void Start()
    {
        leverSpriteRenderer = GetComponent<SpriteRenderer>();

        if (interactionPromptPrefab != null)
        {
            promptInstance = Instantiate(
                interactionPromptPrefab,
                transform.position + promptOffset,
                Quaternion.identity
            );

            promptText = promptInstance.GetComponentInChildren<TMP_Text>();
            
            if (promptText != null)
            {
                promptText.text = "Pull Lever (Press E)";
            }

            promptInstance.SetActive(false);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        // Show the prompt when the player is nearby.
        if (!isLeverPulled &&
            distance <= promptRange &&
            promptInstance != null)
        {
            promptInstance.SetActive(true);
        }
        else if (promptInstance != null)
        {
            promptInstance.SetActive(false);
        }

        // Existing lever interaction.
        if (!isLeverPulled &&
            Input.GetKeyDown(KeyCode.E) &&
            distance < 3f)
        {
            isLeverPulled = true;
            leverSpriteRenderer.sprite = pulledLeverSprite;

            if (promptInstance != null)
            {
                promptInstance.SetActive(false);
            }
        }
    }

    void OnDestroy()
    {
        if (promptInstance != null)
        {
            Destroy(promptInstance);
        }
    }
}
