using System.Collections;
using UnityEngine;


public class FallingBlockTrigger : MonoBehaviour
{

    private bool hasTriggered = false;
    private SpriteRenderer fallingBlockSpriteRenderer; 
    public Sprite idleSprite; 
    public Sprite fallingSprite;
    private Vector3 initialPosition;
    private Rigidbody2D rb;

    void Awake()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        
    //         Debug.Log(
    //     $"[{name}] Awake | Initial Position: {initialPosition} | " +
    //     $"Initial Sprite: {GetComponent<SpriteRenderer>()?.sprite?.name} | " +
    //     $"Rigidbody: {(rb != null ? rb.bodyType.ToString() : "None")}"
    // );
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
        // Debug.Log(
        //     $"[{name}] ResetPosition CALLED | " +
        //     $"Before Position: {transform.position} | " +
        //     $"Before Sprite: {fallingBlockSpriteRenderer?.sprite?.name} | " +
        //     $"Saved Position: {initialPosition}"
        // );

        transform.position = initialPosition;
        hasTriggered = false;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        // Debug.Log(
        //     $"[{name}] ResetPosition FINISHED | " +
        //     $"After Position: {transform.position} | " +
        //     $"After Sprite: {fallingBlockSpriteRenderer?.sprite?.name}"
        // );
    }
}
