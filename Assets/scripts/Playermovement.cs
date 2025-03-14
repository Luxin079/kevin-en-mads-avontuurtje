using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float superJumpHeight = 10f;

    public float superJumpCooldown = 5f;
    private float superJumpTimer = 0f;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    private Vector3 velocity;
    public bool isGrounded;

    // Dash variables
    public float dashDistance = 8f;
    public float dashCooldown = 3f;
    public float dashDuration = 0.2f; // Duration of the dash
    private float dashTimer = 0f;
    private bool isDashing = false;

    // Bunny hopping speed multiplier
    public float bunnyHopMultiplier = 1.5f;
    private bool isBunnyHopping = false;

    // Aiming state
    private bool isAiming = false; // From WeaponModelReload

    void Update()
    {
        // Update timers
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (superJumpTimer > 0) superJumpTimer -= Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isBunnyHopping = false; // Reset bunny hopping when grounded
        }

        // Get player input for movement
        float x = 0;
        float z = 0;

        if (Input.GetKey("a")) x += -1;
        if (Input.GetKey("d")) x += 1;
        if (Input.GetKey("w")) z += 1;
        if (Input.GetKey("s")) z += -1;

        Vector3 move = transform.right * x + transform.forward * z;

        if (!isDashing) // Prevent normal movement while dashing
        {
            float currentSpeed = speed;

            if (isAiming) // Reduce speed while aiming
            {
                currentSpeed *= 0.5f; // Example: reduce speed while scoped in
            }

            if (!isGrounded && velocity.y > 0) // If jumping, apply bunny hop multiplier
            {
                currentSpeed *= bunnyHopMultiplier;
                isBunnyHopping = true;
            }

            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // Dash mechanic with smooth motion
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0 && move.magnitude > 0 && !isDashing)
        {
            StartCoroutine(SmoothDash(move.normalized));
            dashTimer = dashCooldown;
        }

        // Normal jump
        if (Input.GetButton("Jump") && isGrounded && !isAiming) // Disable jump while scoped in
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Super Jump (Q Key) with cooldown
        if (Input.GetKeyDown(KeyCode.Q) && isGrounded && superJumpTimer <= 0)
        {
            velocity.y = Mathf.Sqrt(superJumpHeight * -2f * gravity);
            superJumpTimer = superJumpCooldown;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator SmoothDash(Vector3 direction)
    {
        isDashing = true;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * dashDistance;

        while (elapsedTime < dashDuration)
        {
            controller.Move(direction * (dashDistance / dashDuration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    // Method to set aiming state from WeaponModelReload
    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }
}
