
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float platformSpeed = 5f;
    public float moveDistance = 15f;

    private Vector3 pointA;
    private Vector3 pointB;

    void Start()
    {
        pointA = transform.position;
        pointB = pointA + new Vector3(moveDistance, 0, 0);
    }

    void Update()
    {
        float distance = Mathf.PingPong(
            Time.time * platformSpeed,
            moveDistance
        );

        transform.position = pointA + new Vector3(distance, 0, 0);
    }

    // Moving player with the platform when they are standing on it

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
