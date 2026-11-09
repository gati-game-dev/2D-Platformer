
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class BombBlast : MonoBehaviour
{
    private GameObject player;

    public float explosionRadius = 5f;
    public int damage = 50;

    private bool playerInRange = false;
    private bool hasExploded = false;

    public Tilemap destructibleTilemap;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite; 

    private Dictionary<Vector3Int, TileBase> destroyedTiles =
        new Dictionary<Vector3Int, TileBase>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();

        if (destructibleTilemap == null)
        {
            GameObject tilemapObject = GameObject.Find("Tilemap");

            if (tilemapObject != null)
            {
                destructibleTilemap =
                    tilemapObject.GetComponent<Tilemap>();
            }
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            defaultSprite = spriteRenderer.sprite;
        }    
    }

    void Update()
    {
        // Control the blinking animation.
        if (animator != null)
        {
            animator.SetBool("PlayerInRange", playerInRange);
        }

        // Explode only once when the player enters the radius.
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

    IEnumerator ExplodeAfterDelay()
    {
        // Wait before exploding.
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
                PlayerHealth health =
                    player.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    health.TakeDamage(damage, transform);
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
                    destructibleTilemap.GetCellCenterWorld(
                        cellPosition
                    );

                float distance = Vector2.Distance(
                    transform.position,
                    cellWorldPosition
                );

                // Only consider tiles inside the explosion radius.
                if (distance <= explosionRadius)
                {
                    // 75% chance of destroying each tile.
                    float chance = Random.Range(0f, 1f);

                    if (chance < 0.75f)
                    {
                        TileBase originalTile =
                            destructibleTilemap.GetTile(cellPosition);

                        if (originalTile != null)
                        {
                            // Save the original tile before destroying it.
                            destroyedTiles[cellPosition] = originalTile;

                            destructibleTilemap.SetTile(
                                cellPosition,
                                null
                            );
                        }
                    }
                }
            }
        }
    }

    public void ResetBombDestruction()
    {
        StopAllCoroutines();

        // Restore the tiles destroyed by this bomb.
        if (destructibleTilemap != null)
        {
            foreach (KeyValuePair<Vector3Int, TileBase> entry in destroyedTiles)
            {
                destructibleTilemap.SetTile(entry.Key, entry.Value);
            }
        }

        destroyedTiles.Clear();

        // Reset bomb state.
        playerInRange = false;
        hasExploded = false;

        // Reactivate the bomb first.
        gameObject.SetActive(true);

        // Reset the bomb's animation state.
        if (animator != null)
        {
            animator.SetBool("PlayerInRange", false);
            animator.Play("Bomb_idle", 0, 0f);
            animator.Update(0f);
        }

        // Explicitly restore the correct sprite.
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
        // Restore the normal sprite.
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }

        Debug.Log("Bomb and destroyed tiles reset!");
    }
}
