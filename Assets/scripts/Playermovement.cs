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
    public float dashDuration = 0.2f;
    private float dashTimer = 0f;
    private bool isDashing = false;

    // Air acceleration (bunny hop / air strafing)
    public float airAccelerationRate = 0.2f;
    public float airStrafeAccelerationRate = 0.3f;
    public float airForwardAccelerationRate = 0.2f;
    public float airComboAcceleration = 0.5f;  // Extra boost when strafing + moving forward
    public float airMaxMultiplier = 1.5f;
    public float airDecelerationRate = 0.3f;
    private float currentAirMultiplier = 1f;

    public float consecutiveJumpResetTime = 0.2f;
    private float groundTimer = 0f;

    private bool isAiming = false;

    void Update()
    {
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (superJumpTimer > 0) superJumpTimer -= Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            groundTimer += Time.deltaTime;
            if (groundTimer > consecutiveJumpResetTime)
            {
                currentAirMultiplier = 1f;
            }
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            groundTimer = 0f;

            float x = 0;
            float z = 0;
            if (Input.GetKey("a")) x += -1;
            if (Input.GetKey("d")) x += 1;
            if (Input.GetKey("w")) z += 1;
            if (Input.GetKey("s")) z += -1;
            Vector3 move = transform.right * x + transform.forward * z;

            if (move.magnitude > 0.1f)
            {
                float acceleration = airAccelerationRate;

                if (Mathf.Abs(x) > 0.1f) // Strafing
                {
                    acceleration += airStrafeAccelerationRate;
                }
                if (z > 0.1f) // Moving forward
                {
                    acceleration += airForwardAccelerationRate;
                }
                if (Mathf.Abs(x) > 0.1f && z > 0.1f) // Forward + Strafe Combo Boost
                {
                    acceleration += airComboAcceleration;
                }

                currentAirMultiplier = Mathf.Min(currentAirMultiplier + acceleration * Time.deltaTime, airMaxMultiplier);
            }
            else
            {
                currentAirMultiplier = Mathf.Max(currentAirMultiplier - airDecelerationRate * Time.deltaTime, 1f);
            }
        }

        float inputX = 0;
        float inputZ = 0;
        if (Input.GetKey("a")) inputX += -1;
        if (Input.GetKey("d")) inputX += 1;
        if (Input.GetKey("w")) inputZ += 1;
        if (Input.GetKey("s")) inputZ += -1;
        Vector3 movementInput = transform.right * inputX + transform.forward * inputZ;
        movementInput = Vector3.ClampMagnitude(movementInput, 1f);

        if (!isDashing)
        {
            float currentSpeed = speed;
            if (isAiming)
            {
                currentSpeed *= 0.5f;
            }
            if (!isGrounded)
            {
                currentSpeed *= currentAirMultiplier;
            }

            Vector3 horizontalVelocity = movementInput * currentSpeed;
            float speedCap = isGrounded ? speed : speed * airMaxMultiplier;
            if (horizontalVelocity.magnitude > speedCap)
            {
                horizontalVelocity = horizontalVelocity.normalized * speedCap;
            }

            controller.Move(horizontalVelocity * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0 && movementInput.magnitude > 0 && !isDashing)
        {
            StartCoroutine(SmoothDash(movementInput.normalized));
            dashTimer = dashCooldown;
        }

        if (Input.GetButton("Jump") && isGrounded && !isAiming)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

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
        while (elapsedTime < dashDuration)
        {
            controller.Move(direction * (dashDistance / dashDuration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }
}
