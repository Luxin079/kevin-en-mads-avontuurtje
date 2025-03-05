using UnityEngine;
using System.Collections;

public class WeaponModelReload : MonoBehaviour
{
    public float reloadTime = 2f; // Total time for the reload animation (2 seconds)
    public float reloadRotationAngle = 45f; // Angle to rotate the gun upward during reload
    public int maxAmmo = 6; // Maximum ammo capacity
    public int currentAmmo; // Current ammo count

    private Quaternion initialRotation; // Stores the initial rotation of the weapon
    public bool isReloading = false; // Tracks if the weapon is currently reloading
    private Coroutine reloadCoroutine; // Stores the reload coroutine so it can be stopped

    private Vector3 initialPosition; // Initial position of the weapon
    public Vector3 aimPosition = new Vector3(0f, -0.105f, 0.5f); // ADS position
    public float aimSpeed = 10f; // Speed of ADS transition (fully zoom in 0.3s)
    private bool isAiming = false;

    private void Start()
    {
        // Store the initial rotation and position of the weapon
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;

        // Initialize ammo to max (or any other value you want)
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        // Start reloading when the player presses R, is not already reloading, and ammo is not full
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            reloadCoroutine = StartCoroutine(Reload());
        }

        // Check for shooting input (e.g., left mouse button)
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            // If shooting mid-reload, cancel the reload
            if (isReloading)
            {
                CancelReload();
            }
        }

        // Aim down sights logic (only if not reloading)
        if (!isReloading)
        {
            if (Input.GetMouseButton(1))
            {
                isAiming = true;
            }
            else
            {
                isAiming = false;
            }
        }

        // Smoothly transition between aiming and normal position
        Vector3 targetPosition = isAiming ? aimPosition : initialPosition;
        Quaternion targetRotation = isAiming ? Quaternion.identity : initialRotation;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * aimSpeed);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * aimSpeed);
    }

    private void CancelReload()
    {
        // Stop the reload coroutine
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }

        // Reset the weapon's rotation to its initial state
        transform.localRotation = initialRotation;

        // Reset the reloading flag
        isReloading = false;

        Debug.Log("Weapon Model: Reload canceled!");
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        isAiming = false; // Disable aiming while reloading
        Debug.Log("Weapon Model: Reloading...");

        // Rotate the gun upward
        float elapsedTime = 0f;
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(-reloadRotationAngle, 0f, 0f) * initialRotation; // Negative angle for upward rotation

        while (elapsedTime < reloadTime / 2)
        {
            // Smoothly rotate the gun upward
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / (reloadTime / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Rotate the gun back to its original position
        elapsedTime = 0f;
        startRotation = transform.localRotation;

        while (elapsedTime < reloadTime / 2)
        {
            // Smoothly rotate the gun back down
            transform.localRotation = Quaternion.Slerp(startRotation, initialRotation, elapsedTime / (reloadTime / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Refill ammo after reloading
        currentAmmo = maxAmmo;

        // Finish reloading
        isReloading = false;
        Debug.Log("Weapon Model: Reload Complete!");
    }
}