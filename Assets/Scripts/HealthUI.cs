using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider healthSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (playerHealth != null)
        // {
        //     // Initialize the health slider with the player's current health
        //     healthSlider.value = playerHealth.Health;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthUI(int Health)
    {
        healthSlider.value = playerHealth.Health;
        Debug.Log("Health: " + playerHealth.Health);
        if (healthSlider.value <= 30)
        {
            healthSlider.fillRect.GetComponent<Image>().color = Color.tomato;
        }

    }
}
