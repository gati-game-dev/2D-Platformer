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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        destructibleTilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !hasExploded)
        {
            StartCoroutine(ExplodeAfterDelay());
            hasExploded = true; // Ensure the explosion only happens once
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        // Wait for 1 second before exploding
        yield return new WaitForSeconds(1.5f);
        Explode();
    }

    void Explode()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= explosionRadius)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(50);
        }
        DestroyTiles();
        gameObject.SetActive(false); // Deactivate the bomb after explosion
    }

    void DestroyTiles()
    {
        Vector3Int bombCell = destructibleTilemap.WorldToCell(transform.position);

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
                    // Randomly decide whether this tile gets destroyed
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
