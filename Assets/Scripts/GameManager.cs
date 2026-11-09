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
    public TMP_Text escapeSuccessfulMessage;
    private bool waitingToRestart = false;
    public FallingBlockTrigger[] fallingBlockTriggers;  
    public FallingWeightsTrigger[] fallingWeightTriggers;
    public GameObject spring;
    public BombBlast[] bombBlasts;
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

        foreach (FallingBlockTrigger block in fallingBlockTriggers)
        {
            block.ResetPosition();
        }

        foreach (FallingWeightsTrigger weight in fallingWeightTriggers)
        {
            weight.ResetPosition();
            Debug.Log("weights will reset now");
        }

        foreach (BombBlast bomb in bombBlasts)
        {
            if (bomb != null)
            {
                bomb.ResetBombDestruction();
            }
        }      
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

    public void GameOver()
    {
        Time.timeScale = 0;
        escapeSuccessfulMessage.gameObject.SetActive(true);
        Debug.Log("Game Over: Player has successfully escaped!");

    }
}

