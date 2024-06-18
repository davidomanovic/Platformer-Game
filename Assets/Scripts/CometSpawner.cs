using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab;
    public Transform planet;
    public Transform player;
    public float spawnRadius = 10f;
    public float cometSpeed = 5f;
    public int maxComets = 3;

    private int currentCometCount = 0;

    void Start()
    {
        StartCoroutine(SpawnComets());
    }

    IEnumerator SpawnComets()
    {
        while (true)
        {
            if (currentCometCount < maxComets)
            {
                SpawnComet();
            }
            yield return new WaitForSeconds(Random.Range(3f, 10f)); // Random spawn interval between 3 and 10 seconds
        }
    }

    void SpawnComet()
    {
        // Generate a random angle around the player
        float angle = Random.Range(0, 2 * Mathf.PI);

        // Calculate the spawn position around the player
        Vector2 spawnPosition = new Vector2(
            player.position.x + spawnRadius * Mathf.Cos(angle),
            player.position.y + spawnRadius * Mathf.Sin(angle)
        );

        // Instantiate the comet at the spawn position
        GameObject comet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);

        // Calculate the direction towards the player
        Vector2 directionToPlayer = (player.position - comet.transform.position).normalized;

        // Apply a force to move the comet towards the player
        Rigidbody2D rb = comet.GetComponent<Rigidbody2D>();
        rb.velocity = directionToPlayer * cometSpeed;

        // Increment the comet count
        currentCometCount++;
    }

    public void CometLanded()
    {
        currentCometCount--;
    }
}