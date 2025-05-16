using UnityEngine;

public class gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1.2f; // Time between shots in seconds
    public float reloadTime = 2.0f; // Time it takes to reload

    public Camera fpsCam;
    public ParticleSystem[] muzzleFlash;
    public WeaponModelReload wmrScript;

    private float nextTimeToFire = 0f;

    // 🎵 Audio Configuration
    private AudioSource audioSource; // Audio Source
    public AudioClip gunshotSound; // Gunshot Sound Effect

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        wmrScript.currentAmmo = wmrScript.maxAmmo; // Initialize ammo

        foreach (var mf in muzzleFlash)
        {
            mf.Stop();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && wmrScript.isReloading)
        {
            Debug.Log("must cancel");
            wmrScript.CancelReload();
        }

        if (wmrScript.isReloading) return;

        if (Input.GetKeyDown(KeyCode.R) && wmrScript.currentAmmo < wmrScript.maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && wmrScript.currentAmmo > 0)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Debug.Log("Shoot");
        wmrScript.currentAmmo--;

        foreach (var mf in muzzleFlash)
        {
            mf.Play();
        }

        // 🎵 Play gunshot sound
        if (audioSource != null && gunshotSound != null)
        {
            audioSource.Play();
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

        yield return new WaitForSeconds(reloadTime);

        wmrScript.currentAmmo = wmrScript.maxAmmo;
        wmrScript.isReloading = false;
        Debug.Log("Reload Complete!");
    }
}
