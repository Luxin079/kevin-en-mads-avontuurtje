using UnityEngine;

public class cylinder : MonoBehaviour
{
    public GameObject towerPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTower();
        }
    }

    void SpawnTower()
    {

        Vector3 randomPos = new Vector3(
            Random.Range(-5f, 5f),
            0f,
            Random.Range(-5f, 5f)
        );

        Instantiate(towerPrefab, randomPos, Quaternion.identity);
    }
}