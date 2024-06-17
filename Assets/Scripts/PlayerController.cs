using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float radius;
    public float jumpForce;
    public Transform center;
    public float gravityForce = 9.81f;
    public LayerMask groundLayer; // Add this public variable to set the ground layer

    private Rigidbody2D playerRigidBody;
    private float currentAngle;
    private bool isGrounded;
    private Vector2 gravityDirection;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        currentAngle = Mathf.Atan2(transform.position.y - center.position.y, transform.position.x - center.position.x);
    }

    void Update()
    {
        // Get input direction
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the change in angle based on input and walk speed
        currentAngle += horizontalInput * walkSpeed * Time.deltaTime;

        // Calculate the new position of the player on the circular path
        float x = center.position.x + radius * Mathf.Cos(currentAngle);
        float y = center.position.y + radius * Mathf.Sin(currentAngle);

        // Update the player's position
        transform.position = new Vector3(x, y, transform.position.z);

        // Calculate the rotation angle to make the player perpendicular to the circle
        float angle = Mathf.Atan2(y - center.position.y, x - center.position.x) * Mathf.Rad2Deg;

        // Apply the rotation to the player's transform
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Vector2 jumpDirection = (transform.position - center.position).normalized;
            playerRigidBody.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Apply gravity towards the center of the planet
        gravityDirection = (center.position - transform.position).normalized;
        playerRigidBody.AddForce(gravityDirection * gravityForce);

        // Check if the player is grounded using CircleCast
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, -gravityDirection, 0.2f, groundLayer);
        isGrounded = hit.collider != null;
    }
}
