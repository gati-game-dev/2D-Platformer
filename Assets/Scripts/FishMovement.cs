using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float distance = 5f;
    public float speed = 2f;

    private float startX;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float previousX = transform.position.x;

        float x = startX + Mathf.PingPong(Time.time * speed, distance * 2f) - distance;

        transform.position = new Vector3(
            x,
            transform.position.y,
            transform.position.z
        );

        // Face the direction of movement
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