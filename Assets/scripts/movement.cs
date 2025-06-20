using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;           // Referentie naar Rigidbody
    public float moveSpeed = 5f;   // Snelheid van bewegen
    public float jumpForce = 5f;   // Kracht van sprong

   void Start()
    {
        // Haal Rigidbody op
        rb = GetComponent<Rigidbody>();
        Debug.Log("Speler klaar!");
    }

    void Update()
    {
        //Zorg dat het blokje naar links en rechts kan bewegen met de snelheid moveSpeed
        float input = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * input * moveSpeed * Time.deltaTime;

        // Omhoog springen met spatie en een AddForce op de rigidbody
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}