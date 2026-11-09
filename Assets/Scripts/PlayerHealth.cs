using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private GameManager gameManager;
    private int health = 100;
    public int Health { get { return health; } }    // Other classes can access (but not modify) the player's health
    public HealthUI healthUI; 
    public GameObject damagePopupPrefab;
    public Vector3 popupOffset = new Vector3(0f, 1.5f, 0f);
    private PlayerMovement playerMovement;
    
    
    void Start()
    {
        gameManager = GameManager.instance;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage, Transform damageSource = null)
    {
        health -= damage;
        playerMovement.StartHit();
        playerMovement.ApplyKnockback(damageSource);

        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(
                damagePopupPrefab,
                transform.position + popupOffset,
                Quaternion.identity
            );

            popup.GetComponent<DamagePopup>().Setup(damage);
        }

        if (health <= 0)
        {
            gameManager.Die();
        }

        healthUI.UpdateHealthUI(Health);
    }
}
