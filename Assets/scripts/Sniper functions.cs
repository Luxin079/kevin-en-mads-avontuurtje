using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.Security; // Required for handling UI elements

public class WeaponModelReload : MonoBehaviour
{
    public float reloadTime = 2f;
    public float reloadRotationAngle = 45f;
    public int maxAmmo = 6;
    public int currentAmmo;

    private Quaternion initialRotation;
    public bool isReloading = false;
    private Coroutine reloadCoroutine;

    private Vector3 initialPosition;
    public Vector3 aimPosition = new Vector3(0f, -0.105f, 0.5f);
    public float aimSpeed = 10f;
    private bool isAiming = false;
    public float aimThreshold = 0.01f;

    private Renderer[] allRenderers;
    private Camera playerCamera;
    public float defaultFOV = 65f; // Changed default FOV to 65
    public float scopedFOV = 30f;
    public float fovTransitionSpeed = 8f; // Smooth transition speed

    public float currentFOV; // Tracks current FOV

    private PlayerMovement playerMovement; // Reference to player's movement script

    private void Start()
    {
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
        currentAmmo = maxAmmo;

        playerCamera = Camera.main;
        if (playerCamera != null)
        {
            currentFOV = playerCamera.fieldOfView; // Set initial FOV
        }

        // Get all renderers except for Glow, Fire, and Sparks
        Renderer[] allChildrenRenderers = GetComponentsInChildren<Renderer>();
        allRenderers = System.Array.FindAll(allChildrenRenderers, renderer =>
            !renderer.gameObject.name.Contains("Glow") &&
            !renderer.gameObject.name.Contains("Fire") &&
            !renderer.gameObject.name.Contains("Sparks")
        );

        playerMovement = GetComponentInParent<PlayerMovement>(); // Assuming the movement script is on the player object
    }


    public void PlayReload()
    {

        reloadCoroutine = StartCoroutine(Reload());
    }




    private void Update()
    {
        /*
        
        Debug.Log(" false? "+ isReloading);
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {

            Debug.Log("!!");
            reloadCoroutine = StartCoroutine(Reload());
        }

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            if (isReloading)
            {
                CancelReload();
            }
        }*/

        if (!isReloading)
        {
            isAiming = Input.GetMouseButton(1);
        }

        // Freeze movement when aiming
        if (playerMovement != null)
        {
            playerMovement.SetAiming(isAiming); // Set the aiming state
        }

        Vector3 targetPosition = isAiming ? aimPosition : initialPosition;
        Quaternion targetRotation = isAiming ? Quaternion.identity : initialRotation;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * aimSpeed);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * aimSpeed);

        bool isFullyScopedIn = Vector3.Distance(transform.localPosition, aimPosition) < aimThreshold;

        // Hide all parts except effects
        foreach (Renderer part in allRenderers)
        {
            if (part != null)
            {
                part.enabled = !isFullyScopedIn;
            }
        }

        // Smoothly transition the FOV
        float targetFOV = isAiming ? scopedFOV : defaultFOV;//30 65



        currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * fovTransitionSpeed);


        if (playerCamera != null)
        {
            playerCamera.fieldOfView = currentFOV;
        }



    }

    public void CancelReload()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }

        transform.localRotation = initialRotation;
        isReloading = false;
        Debug.Log("Weapon Model: Reload canceled!");
    }

    private IEnumerator Reload()
    {

        Debug.Log("@@");
        isReloading = true;
        isAiming = false;
        Debug.Log("Weapon Model: Reloading...");


        float elapsedTime = 0f;
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(-reloadRotationAngle, 0f, 0f) * initialRotation;

        while (elapsedTime < reloadTime / 2)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / (reloadTime / 2));
            elapsedTime += Time.deltaTime;
            yield return null;

        }

        elapsedTime = 0f;
        startRotation = transform.localRotation;

        while (elapsedTime < reloadTime / 2)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, initialRotation, elapsedTime / (reloadTime / 2));
            elapsedTime += Time.deltaTime;

            yield return null;
        }


        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Weapon Model: Reload Complete!");
    }


}