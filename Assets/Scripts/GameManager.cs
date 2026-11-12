
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    private int keyCollected = 0;
    public int KeyCollected { get { return keyCollected; } }
    private int savedKeyCount = 0;
    private KeyCollected keyCollectedUI;

    public TMP_Text youDiedText;
    public TMP_Text pressRtext;
    public TMP_Text escapeSuccessfulMessage;

    private bool waitingToRestart = false;

    public FallingBlockTrigger[] fallingBlockTriggers;
    public FallingWeightsTrigger[] fallingWeightTriggers;

    public GameObject spring;

    public BombBlast[] bombBlasts;

    private int currentCheckpointIndex = 0;
    private Vector3 checkpointPosition;
    private bool hasCheckpoint = false;


    void Awake()
    {
        // Prevent duplicate GameManagers
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        // Subscribe to scene loading
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDestroy()
    {
        // Unsubscribe when the persistent GameManager is destroyed
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }


    // Called whenever a scene finishes loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded. Reassigning GameManager references.");

        // Find Player
        player = null;

        Transform[] allTransforms = FindSceneComponents<Transform>(scene);

        foreach (Transform obj in allTransforms)
        {
            if (obj.CompareTag("Player"))
            {
                player = obj.gameObject;
                break;
            }
        }

        if (player == null)
        {
            Debug.LogError("Could not find Player!");
        }

        // Respawn player at the saved checkpoint
        if (hasCheckpoint && player != null)
        {
            // Restore saved progress
            keyCollected = savedKeyCount;

            keyCollectedUI = FindSceneComponents<KeyCollected>(scene)
                .FirstOrDefault();

            if (keyCollectedUI != null)
            {
                keyCollectedUI.RefreshKeyUI();
            }

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.position = checkpointPosition;
            }
            else
            {
                player.transform.position = checkpointPosition;
            }

            Debug.Log("Player respawned at checkpoint: " + checkpointPosition);
        }

        // Find UI references
        youDiedText = FindSceneTMPText(scene, "YouDiedText");

        pressRtext = FindSceneTMPText(scene, "PressRtext");

        escapeSuccessfulMessage = FindSceneTMPText(
            scene, "EscapeSuccessfulText"
        );

        // Find Spring parent GameObject
        spring = FindSceneGameObject(scene, "Spring");

        // Find all Falling Block Triggers
        fallingBlockTriggers =
            FindSceneComponents<FallingBlockTrigger>(scene);

        // Find all Falling Weight Triggers
        fallingWeightTriggers =
            FindSceneComponents<FallingWeightsTrigger>(scene);

        // Find all Bomb Blasts
        bombBlasts = FindSceneComponents<BombBlast>(scene);

        Debug.Log("GameManager references reassigned.");
    }


    // Find a GameObject by name, including inactive objects
    GameObject FindSceneGameObject(Scene scene, string objectName)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();

        foreach (GameObject root in rootObjects)
        {
            Transform[] children =
                root.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child.gameObject.name == objectName)
                {
                    return child.gameObject;
                }
            }
        }

        Debug.LogError("Could not find GameObject: " + objectName);
        return null;
    }


    // Find a TMP text component by its GameObject name
    TMP_Text FindSceneTMPText(Scene scene, string objectName)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();

        foreach (GameObject root in rootObjects)
        {
            TMP_Text[] texts =
                root.GetComponentsInChildren<TMP_Text>(true);

            foreach (TMP_Text text in texts)
            {
                if (text.gameObject.name == objectName)
                {
                    return text;
                }
            }
        }

        Debug.LogError("Could not find TMP Text: " + objectName);
        return null;
    }


    // Find all components of a particular type in the loaded scene
    T[] FindSceneComponents<T>(Scene scene) where T : Component
    {
        List<T> sceneComponents = new List<T>();

        GameObject[] rootObjects = scene.GetRootGameObjects();

        foreach (GameObject root in rootObjects)
        {
            T[] components =
                root.GetComponentsInChildren<T>(true);

            foreach (T component in components)
            {
                sceneComponents.Add(component);
            }
        }

        return sceneComponents.ToArray();
    }


    void Update()
    {
        if (waitingToRestart == true && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
            waitingToRestart = false;
        }
    }

    public void SetCheckpoint(int index, Vector3 position)
    {
        // Don't allow the player to go back to an earlier checkpoint
        if (index <= currentCheckpointIndex)
            return;

        currentCheckpointIndex = index;
        checkpointPosition = position;
        hasCheckpoint = true;

        // Save key progress at this checkpoint
        savedKeyCount = keyCollected;

        Debug.Log("Checkpoint activated: " + currentCheckpointIndex);
        Debug.Log("Checkpoint position: " + checkpointPosition);
    }

    public void UpdateKeyCount()
    {
        keyCollected += 1;

        spring.gameObject.SetActive(true);

        foreach (FallingBlockTrigger block in fallingBlockTriggers)
            block.ResetPosition();

        foreach (FallingWeightsTrigger weight in fallingWeightTriggers)
            weight.ResetPosition();

        foreach (BombBlast bomb in bombBlasts)
        {
            if (bomb != null)
                bomb.ResetBombDestruction();
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
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GameOver()
    {
        if (keyCollected > 0)
        {
            Time.timeScale = 0;
            escapeSuccessfulMessage.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Game Over: Player has failed to escape.");
        }
    }
}
