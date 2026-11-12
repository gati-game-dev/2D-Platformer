
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyCollected : MonoBehaviour
{
    private GameManager gameManager;

    public Image keyUI;
    public TMP_Text keysCollectedText;

    void Start()
    {
        gameManager = GameManager.instance;
        RefreshKeyUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateKeyCount();

            RefreshKeyUI();

            Destroy(gameObject);

            Debug.Log("key collected");
        }
    }

    public void RefreshKeyUI()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }

        if (gameManager == null)
            return;

        keysCollectedText.text = gameManager.KeyCollected + "/1";

        keyUI.color = gameManager.KeyCollected >= 1
            ? Color.white
            : Color.gray;
    }
}
