using Unity.VisualScripting;
using UnityEngine;

public class CylinderSpawner : MonoBehaviour
{
    private float elapsedTime = 0f;
    private GameObject cylinder;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randPos = RandomPosition(-10f, 10f);
            CreateCylinder(color, randPos);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1f)
        {
            Vector3 randPos = RandomPosition(-10f, 10f);
            CreateCylinder(color, randPos);

            elapsedTime = 0f;
        }

        Vector3 RandomPosition(float min, float max)
        {
            return new Vector3(Random.Range(min, max), Random.Range(min, max), 0f);
        }

        GameObject CreateCylinder(Color color, Vector3 position)
        {
            cylinder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cylinder.transform.position = position;
            cylinder.GetComponent<Renderer>().material.color = color;
            cylinder.AddComponent<Rigidbody>();

        }




    }


