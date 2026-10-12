using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class BombBlast : MonoBehaviour
{
    private GameObject player;

    public float explosionRadius = 5f;

    private bool playerInRange = false;
    private bool hasExploded = false;

    public Tilemap destructibleTilemap;

    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();

        if (destructibleTilemap == null)
        {
            GameObject tilemapObject = GameObject.Find("Tilemap");

            if (tilemapObject != null)
            {
                destructibleTilemap = tilemapObject.GetComponent<Tilemap>();
            }
        }
    }

    void Update()
    {
        // Start or stop the blinking animation
        if (animator != null)
        {
            animator.SetBool("PlayerInRange", playerInRange);
        }

        // Explode only once when the player enters the radius
        if (playerInRange && !hasExploded)
        {
            hasExploded = true;
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasExploded)
        {
            playerInRange = true;
        }
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         playerInRange = false;
    //     }
    // }

    IEnumerator ExplodeAfterDelay()
    {
        // Wait before exploding
        yield return new WaitForSeconds(1.5f);

        Explode();
    }

    void Explode()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(
                transform.position,
                player.transform.position
            );

            if (distanceToPlayer <= explosionRadius)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    health.TakeDamage(50);
                }
            }
        }

        if (destructibleTilemap != null)
        {
            DestroyTiles();
        }

        gameObject.SetActive(false);
    }

    void DestroyTiles()
    {
        Vector3Int bombCell =
            destructibleTilemap.WorldToCell(transform.position);

        int radius = Mathf.CeilToInt(explosionRadius);

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int cellPosition =
                    bombCell + new Vector3Int(x, y, 0);

                Vector3 cellWorldPosition =
                    destructibleTilemap.GetCellCenterWorld(cellPosition);

                float distance = Vector2.Distance(
                    transform.position,
                    cellWorldPosition
                );

                // Only consider tiles inside the explosion radius
                if (distance <= explosionRadius)
                {
                    // 75% chance of destroying each tile
                    float chance = Random.Range(0f, 1f);

                    if (chance < 0.75f)
                    {
                        destructibleTilemap.SetTile(cellPosition, null);
                    }
                }
            }
        }
    }
}