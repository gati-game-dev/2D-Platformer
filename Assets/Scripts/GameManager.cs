using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    private int keyCollected = 0; 
    public int KeyCollected { get { return keyCollected; } }
    public TMP_Text youDiedText;
    public TMP_Text pressRtext;
    private bool waitingToRestart = false;

    public GameObject spring;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject); // Persist the GameManager across scenes
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (waitingToRestart == true && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
            waitingToRestart = false;
        }
    }

    public void UpdateKeyCount()
    {
        keyCollected += 1;
        spring.gameObject.SetActive(true);
        
    }

    public void Die()
    {
        Time.timeScale = 0;
        Debug.Log("Player has died.");
        youDiedText.gameObject.SetActive(true);
        pressRtext.gameObject.SetActive(true);

        waitingToRestart = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

}
