
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

    public Transform respawnPoint;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
            return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            Vector3 spawnPosition = respawnPoint != null
                ? respawnPoint.position
                : transform.position;

            GameManager.instance.SetCheckpoint(
                checkpointIndex,
                spawnPosition
            );
        }
    }
}
