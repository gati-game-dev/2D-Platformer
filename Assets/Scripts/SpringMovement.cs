using UnityEngine;

public class SpringMovement : MonoBehaviour
{
    public Sprite SpringExtended;
    public SpriteRenderer spriteRenderer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.isTouchingSpring = true;
                spriteRenderer.sprite = SpringExtended;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.isTouchingSpring = false;
            }
        }
    }
}