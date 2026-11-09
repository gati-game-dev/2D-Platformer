
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float moveSpeed = 1f;
    public float lifetime = 1f;

    [Header("Damage Text")]
    public TMP_Text damageText;

    private CanvasGroup canvasGroup;
    private float timer;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(int damage)
    {
        damageText.text = "-" + damage;
    }

    void Update()
    {
        // Move upward
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Track lifetime
        timer += Time.deltaTime;

        // Fade out
        canvasGroup.alpha = 1f - (timer / lifetime);

        // Destroy after lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
