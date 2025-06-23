using System.Collections;
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

    [Header("Dash Settings")]
    public float dashDistance = 8f;
    public float dashCooldown = 3f;
    public float dashDuration = 0.2f;
    private float dashTimer = 0f;
    private bool isDashing = false;

    [Header("Slide Settings")]
    public float slideSpeedBoost = 2f;
    public float slideDownhillMultiplier = 1.5f;
    public float maxSlideSpeed = 20f;
    public float slideCooldown = 0.5f;
    public float slideMinSpeed = 5f;
    private float slideTimer = 0f;
    private bool isSliding = false;
    private bool slideQueued = false;
    private Vector3 slideDirection;

    public Transform playerModel;
    public Transform cameraTransform;
    public float slideTiltAngle = 45f;
    public float slideTiltSpeed = 10f;
    public float slideCameraHeightOffset = -0.5f;
    public float cameraReturnSpeed = 5f;
    private float originalCameraY;

    [Header("Air Movement")]
    public float airAccelerationRate = 0.2f;
    public float airStrafeAccelerationRate = 0.3f;
    public float airForwardAccelerationRate = 0.2f;
    public float airComboAcceleration = 0.5f;
    public float airMaxMultiplier = 1.5f;
    public float airDecelerationRate = 0.3f;
    private float currentAirMultiplier = 1f;

    public float consecutiveJumpResetTime = 0.2f;
    private float groundTimer = 0f;

    private bool isAiming = false;

    void Start()
    {
        if (cameraTransform != null)
            originalCameraY = cameraTransform.localPosition.y;
    }

    void Update()
    {
        // Update timers
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (superJumpTimer > 0) superJumpTimer -= Time.deltaTime;
        if (slideTimer > 0) slideTimer -= Time.deltaTime;

        // Ground check
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

            // Execute queued slide when landing
            if (slideQueued)
            {
                slideQueued = false;
                TryStartSlide();
            }
        }
        else
        {
            groundTimer = 0f;
            HandleAirMovement();
        }

        // Slide input (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && slideTimer <= 0 && !isSliding && !isDashing)
        {
            TryStartSlide();
        }

        Vector3 movementInput = GetMovementInput();

        if (!isDashing && !isSliding)
        {
            MovePlayer(movementInput);
        }

        // Dash (E key)
        if (Input.GetKeyDown(KeyCode.E) && dashTimer <= 0 && movementInput.magnitude > 0 && !isDashing && !isSliding)
        {
            StartCoroutine(SmoothDash(movementInput.normalized));
            dashTimer = dashCooldown;
        }

        // Normal jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isAiming)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Super jump (Ctrl key)
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded && superJumpTimer <= 0)
        {
            velocity.y = Mathf.Sqrt(superJumpHeight * -2f * gravity);
            superJumpTimer = superJumpCooldown;
        }

        // Apply gravity and move
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset model tilt
        if (!isSliding && playerModel != null)
        {
            playerModel.localRotation = Quaternion.Slerp(playerModel.localRotation, Quaternion.identity, slideTiltSpeed * Time.deltaTime);
        }

        // Reset camera Y-position
        if (!isSliding && cameraTransform != null)
        {
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, originalCameraY, cameraReturnSpeed * Time.deltaTime);
            cameraTransform.localPosition = camPos;
        }
    }

    void TryStartSlide()
    {
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        if (horizontalVelocity.magnitude < slideMinSpeed && !isGrounded)
        {
            slideQueued = true;
            return;
        }

        Vector3 movementInput = GetMovementInput();
        slideDirection = movementInput.magnitude > 0.1f ? movementInput.normalized : transform.forward;

        StartCoroutine(PerformSlide());
        slideTimer = slideCooldown;
    }

    IEnumerator PerformSlide()
    {
        isSliding = true;

        // Slide setup
        if (playerModel != null)
            playerModel.localRotation = Quaternion.Euler(slideTiltAngle, 0f, 0f);

        if (cameraTransform != null)
        {
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y += slideCameraHeightOffset;
            cameraTransform.localPosition = camPos;
        }

        // Calculate initial slide velocity
        Vector3 slideVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        if (slideVelocity.magnitude < speed)
            slideVelocity = slideDirection * speed;

        slideVelocity = slideDirection * slideVelocity.magnitude * slideSpeedBoost;

        float slideTime = Mathf.Clamp(1f - (slideVelocity.magnitude / maxSlideSpeed), 0.3f, 0.6f);
        float elapsedTime = 0f;

        while (elapsedTime < slideTime && Input.GetKey(KeyCode.LeftShift))
        {
            // Downhill boost
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f, groundMask))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle > 5f)
                {
                    float slopeFactor = Mathf.Clamp01(slopeAngle / 45f) * slideDownhillMultiplier;
                    Vector3 downhillForce = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized * slopeFactor;
                    slideVelocity += downhillForce * Time.deltaTime * 15f;
                }
            }

            // Speed limit
            if (slideVelocity.magnitude > maxSlideSpeed)
            {
                slideVelocity = slideVelocity.normalized * maxSlideSpeed;
            }

            // Apply movement
            controller.Move((slideVelocity + Vector3.up * velocity.y) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // End slide
        isSliding = false;
        slideQueued = false;
    }

    void HandleAirMovement()
    {
        float x = 0, z = 0;
        if (Input.GetKey("a")) x += -1;
        if (Input.GetKey("d")) x += 1;
        if (Input.GetKey("w")) z += 1;
        if (Input.GetKey("s")) z += -1;
        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude > 0.1f)
        {
            float acceleration = airAccelerationRate;
            if (Mathf.Abs(x) > 0.1f) acceleration += airStrafeAccelerationRate;
            if (z > 0.1f) acceleration += airForwardAccelerationRate;
            if (Mathf.Abs(x) > 0.1f && z > 0.1f) acceleration += airComboAcceleration;

            currentAirMultiplier = Mathf.Min(currentAirMultiplier + acceleration * Time.deltaTime, airMaxMultiplier);
        }
        else
        {
            currentAirMultiplier = Mathf.Max(currentAirMultiplier - airDecelerationRate * Time.deltaTime, 1f);
        }

        // Queue slide if in air
        if (Input.GetKeyDown(KeyCode.LeftShift) && !slideQueued)
        {
            slideQueued = true;
            slideDirection = GetMovementInput().normalized;
            if (slideDirection.magnitude < 0.1f)
            {
                slideDirection = transform.forward;
            }
        }
    }

    Vector3 GetMovementInput()
    {
        float inputX = 0, inputZ = 0;
        if (Input.GetKey("a")) inputX += -1;
        if (Input.GetKey("d")) inputX += 1;
        if (Input.GetKey("w")) inputZ += 1;
        if (Input.GetKey("s")) inputZ += -1;
        Vector3 input = transform.right * inputX + transform.forward * inputZ;
        return Vector3.ClampMagnitude(input, 1f);
    }

    void MovePlayer(Vector3 movementInput)
    {
        float currentSpeed = speed;
        if (isAiming) currentSpeed *= 0.5f;
        if (!isGrounded) currentSpeed *= currentAirMultiplier;

        Vector3 horizontalVelocity = movementInput * currentSpeed;
        float speedCap = isGrounded ? speed : speed * airMaxMultiplier;
        if (horizontalVelocity.magnitude > speedCap)
        {
            horizontalVelocity = horizontalVelocity.normalized * speedCap;
        }

        controller.Move(horizontalVelocity * Time.deltaTime);
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