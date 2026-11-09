using System.Collections;
using UnityEngine;


public class FallingBlockTrigger : MonoBehaviour
{

    private bool hasTriggered = false;
    private SpriteRenderer fallingBlockSpriteRenderer; 
    public Sprite idleSprite; 
    public Sprite fallingSprite;
    public Sprite restingSprite;
    private Vector3 initialPosition;
    public Rigidbody2D rb;

    void Awake()
    {
        initialPosition = transform.position;

    }
    void Start()
    {        
        fallingBlockSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            fallingBlockSpriteRenderer.sprite = idleSprite;
            StartCoroutine(FallAfterDelay());
        }

        if (other.gameObject.CompareTag("Ground") && hasTriggered)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator FallAfterDelay()
    {
        // Wait 2 seconds after the player steps on it
        yield return new WaitForSeconds(1f);

        // Keep moving downward
        // Change sprite when the block starts falling
        fallingBlockSpriteRenderer.sprite = fallingSprite;

        // Start falling

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void ResetPosition()
    { 
        gameObject.SetActive(true);
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = initialPosition;
        hasTriggered = false;
        fallingBlockSpriteRenderer.sprite = restingSprite;
    }
}
