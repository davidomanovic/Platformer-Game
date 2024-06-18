using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform planet;
    private float moveSpeed = 3f;
    private float jumpForce = 0.5f;
    public float gravity = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector2 tangent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float horizontalInput = -1*Input.GetAxis("Horizontal");

        // Calculate the tangent direction to the planet's surface
        Vector2 planetToPlayer = (transform.position - planet.position).normalized;
        tangent = new Vector2(-planetToPlayer.y, planetToPlayer.x); // Perpendicular vector to planetToPlayer

        // Apply horizontal input to move clockwise or counterclockwise
        Vector2 targetVelocity = tangent * horizontalInput * moveSpeed;
        rb.velocity = targetVelocity + Vector2.Dot(rb.velocity, planetToPlayer) * planetToPlayer; // Maintain existing vertical velocity component
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Vector2 gravityDirection = (transform.position - planet.position).normalized;
            Vector2 jumpDirection = gravityDirection * jumpForce;

            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        Vector2 gravityDirection = (planet.position - transform.position).normalized;
        rb.AddForce(gravityDirection * gravity * rb.mass);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == planet.gameObject)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == planet.gameObject)
        {
            isGrounded = false;
        }
    }
}
