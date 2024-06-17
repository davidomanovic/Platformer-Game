using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target; // Assign the target circle in the Unity Inspector
    public float gravityStrength = 9.8f; // Strength of gravity
    public float bulletSpeed = 5f; // Speed of the bullet

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Camera cam;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomSpawnPoint = GetRandomSpawnPoint();
        transform.position = randomSpawnPoint;
        Vector2 directionToTarget = ((Vector2)target.position - randomSpawnPoint).normalized;
        rb.velocity = directionToTarget * bulletSpeed;
        
        // Calculate the rotation angle
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
    }
    
    void Update()
    {
        // Continuously move the bullet based on its velocity
        transform.position += (Vector3)rb.velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Planet"))
        {
            // Destroy the current asteroid
            Destroy(gameObject);

            // Respawn a new asteroid after a delay
            StartCoroutine(RespawnAsteroid());
        }
    }

    private IEnumerator RespawnAsteroid()
    {
        yield return new WaitForSeconds(2);

        // Create a new asteroid from the prefab
        Instantiate(gameObject, GetRandomSpawnPoint(), Quaternion.identity);
    }

    private Vector2 GetRandomSpawnPoint()
    {
        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        // Add some padding to ensure the asteroid spawns slightly outside the camera view
        float padding = 0.2f;

        float minX = -cameraWidth - padding;
        float maxX = cameraWidth + padding;
        float randomX = Random.Range(minX, maxX);

        float minY = -cameraHeight - padding;
        float maxY = cameraHeight + padding;
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
}