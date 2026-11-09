
using UnityEngine;

public class FallingWeightsTrigger : MonoBehaviour
{
    private Transform player;
    public float triggerZone = 2f; 
    public int damageAmount = 10;
    private Vector3 initialPosition;
    private bool hasTriggered = false;
    public Rigidbody2D rb;

    void Awake()
    {
        initialPosition = transform.position;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) <= triggerZone
            && !hasTriggered)
        {
            hasTriggered = true;

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
    

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount, transform); // Call the TakeDamage method on the PlayerHealth script
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }


    public void ResetPosition()
    { 
        transform.position = initialPosition;
        rb.bodyType = RigidbodyType2D.Static;
        hasTriggered = false;
        Debug.Log("RESETTED");
    }
}

