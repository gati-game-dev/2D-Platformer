using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider healthSlider;
    void Start()
    {
    }
    void Update()
    {
        
    }

    public void UpdateHealthUI(int Health)
    {
        healthSlider.value = playerHealth.Health;

        if (healthSlider.value <= 30)
        {
            healthSlider.fillRect.GetComponent<Image>().color = Color.tomato;
        }

    }
}
