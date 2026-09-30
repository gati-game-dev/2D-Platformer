using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float platformSpeed = 5f;

    private Vector3 pointA;
    private Vector3 pointB;

    void Start()
    {
        pointA = transform.position;
        pointB = pointA + new Vector3(15, 0, 0);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            pointB,
            platformSpeed * Time.deltaTime
        );

        if (transform.position == pointB)
        {
            pointB = pointA;
            pointA = transform.position;
        }
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
