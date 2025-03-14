using UnityEngine;

public class gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1.2f; // Time between shots in seconds
                                  //  public int magazineSize = 6; // Number of bullets in a magazine
    public float reloadTime = 2.0f; // Time it takes to reload

    public Camera fpsCam;
    public ParticleSystem[] muzzleFlash;

    public WeaponModelReload wmrScript;

    //private int currentAmmo; // Current ammo in the magazine
    private float nextTimeToFire = 0f; // Tracks when the next shot can be fired
                                       //private bool isReloading = false; // Tracks if the gun is currently reloading



    private void Start()
    {
        wmrScript.currentAmmo = wmrScript.maxAmmo; // Initialize ammo

        foreach (var mf in muzzleFlash)
        {
            mf.Stop();
        }
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1") && wmrScript.isReloading) {
            //cancel

            Debug.Log("must cancel");
            wmrScript.CancelReload();
            //wmrScript.PlayCancel();


        }

        // Do nothing if reloading
        if (wmrScript.isReloading)
            return;

        // Reload if pressing the reload button (R) and not already at full ammo
        if (Input.GetKeyDown(KeyCode.R) && wmrScript.currentAmmo < wmrScript.maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        // Check if the fire button is pressed, if enough time has passed since the last shot, and if there is ammo
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && wmrScript.currentAmmo > 0)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate; // Set the next time the player can shoot
        }



    }

    void Shoot()
    {

        Debug.Log("Shoot");
        wmrScript.currentAmmo--; // Reduce ammo by 1

        foreach (var mf in muzzleFlash)
        {
            mf.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    System.Collections.IEnumerator Reload()
    {

        wmrScript.isReloading = true;
        wmrScript.PlayReload();
        Debug.Log("Reloading...");

        // Simulate reload time (2 seconds)
        yield return new WaitForSeconds(reloadTime);

        // Reset ammo and finish reloading

        wmrScript.currentAmmo = wmrScript.maxAmmo;

        wmrScript.isReloading = false;
        Debug.Log("Reload Complete!");
    }
}