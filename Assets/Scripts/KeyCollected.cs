using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyCollected : MonoBehaviour
{
    private GameManager gameManager;
    public Image keyUI;  // Reference to the UI image element
    public TMP_Text keysCollectedText;  // Reference to the UI text element

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateKeyCount();
            Destroy(gameObject);
            KeyUI();
        }
    }

    void KeyUI()
    {
        keysCollectedText.text = "1/1";
        keyUI.color = gameManager.KeyCollected >= 1 ? Color.white : Color.gray3;
    }
}
