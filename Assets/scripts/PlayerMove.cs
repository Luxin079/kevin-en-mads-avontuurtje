using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private GameObject enemyposition;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 move = new Vector3(moveX, 0f, moveZ);
        transform.Translate(move);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        GameObject bulletInstance = Instantiate(bulletprefab, transform.position, transform.rotation);

        // Set the bullet's target
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        bulletScript.target = enemyposition.transform;
    }
}
