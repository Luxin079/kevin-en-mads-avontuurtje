using UnityEngine;

public class MoveOverField : MonoBehaviour
{
    [SerializeField] private GameObject movepoint1;
    [SerializeField] private GameObject movepoint2;

    [SerializeField] private float moveSpeed = 3f;

    private bool moveToPoint1 = false;

    void Update()
    {
        Vector3 target = moveToPoint1 ? movepoint1.transform.position : movepoint2.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            moveToPoint1 = !moveToPoint1;
        }
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}