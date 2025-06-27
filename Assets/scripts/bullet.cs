using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public float speed = 20f;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if reached target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // You could trigger hit effects here
            Destroy(gameObject);
        }
    }
}
