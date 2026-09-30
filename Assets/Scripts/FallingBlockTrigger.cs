using System.Collections;
using UnityEngine;

public class FallingBlockTrigger : MonoBehaviour
{

    private bool hasTriggered = false;
    private SpriteRenderer fallingBlockSpriteRenderer; 
    public Sprite idleSprite; 
    public Sprite fallingSprite;

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
    }

    IEnumerator FallAfterDelay()
    {
        // Wait 2 seconds after the player steps on it
        yield return new WaitForSeconds(1f);

        // Keep moving downward
        // Change sprite when the block starts falling
        fallingBlockSpriteRenderer.sprite = fallingSprite;

        // Start falling
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
