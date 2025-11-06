using System.Collections.Generic;
using UnityEngine;

public class SimpleRecoil : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn batch")]
    public int count = 100;
    public Vector3 spawnCenter = Vector3.zero;
    public float spawnJitter = 0.3f;

    [Header("Explosion")]
    public float explosionForce = 15f;
    public float explosionRadius = 8f;
    public float upwardModifier = 0.5f;
    public ForceMode forceMode = ForceMode.Impulse;

    [Header("Auto drip")]
    public float everySeconds = 3f;
    public int perTick = 3;

    private float t;


    public SimpleRecoil cameraRecoil;


    private void Start()
    {
        t += Time.deltaTime;
        if (t >= everySeconds)
        {
            t = 0f;
            Vector3 center = spawnCenter;
            for (int i = 0; i < perTick; i++) SpawnOne(center);
            Explode(center);
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 center = spawnCenter;
            SpawnBatch(center);
            Explode(center);
        }




        if (Input.GetKeyDown(KeyCode.Q)) ClearAll();
    }

    List<GameObject> enemies = new();

    void SpawnBatch(Vector3 center)
    {
        for (int i = 0; i < count; i++) SpawnOne(center);
    }

    void SpawnOne(Vector3 center)
    {

        Vector3 pos = center + Random.insideUnitSphere * spawnJitter;
        pos.y = Mathf.Max(0f, pos.y);
        GameObject e = Instantiate(enemyPrefab, pos, Random.rotation);
        enemies.Add(e);
    }

    void Explode(Vector3 center)
    {

        Collider[] cols = Physics.OverlapSphere(center, explosionRadius);
        foreach (var c in cols)
        {
            Rigidbody rb = c.attachedRigidbody;
            if (rb != null) rb.AddExplosionForce(explosionForce, center, explosionRadius, upwardModifier, forceMode);
        }



    }

    void ClearAll()
    {
        foreach (var e in enemies) if (e) Destroy(e);
        enemies.Clear();
    }
}