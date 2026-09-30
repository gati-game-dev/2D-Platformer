using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.Die();
        }
    }
}
