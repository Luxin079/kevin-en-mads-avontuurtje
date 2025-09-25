using Unity.VisualScripting;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    private float elapsedTime = 0f;
    private GameObject ball;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Color color = RandomColor();
            Vector3 randPos = RandomPosition(-10f, 10f);
            CreateBall(color, randPos);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1f)
        {
            Color color = RandomColor();
            Vector3 randPos = RandomPosition(-10f, 10f);
            CreateBall(color, randPos);

            elapsedTime = 0f;
        }

        if(elapsedTime > 3f)
        {
            Destroy(ball);
        }

     
    }

    Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    Vector3 RandomPosition(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), 0f);
    }

    GameObject CreateBall(Color color, Vector3 position)
    {
         ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.position = position;
        ball.GetComponent<Renderer>().material.color = color;
        ball.AddComponent<Rigidbody>();

        Destroy(ball, 3f);

        return ball;

    }



}
