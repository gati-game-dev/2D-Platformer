using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private GameManager gameManager;
    private int health = 100;
    public int Health { get { return health; } }    // Other classes can access (but not modify) the player's health
    public HealthUI healthUI; 
    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameManager.Die();
        }
        healthUI.UpdateHealthUI(Health); // Update the health UI

    }
}
