using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float moveSpeed;
    public float sprintSpeed;

    void Update()
    {
        //Assuming this is 3rd-person movement and the default Input Manager configuration is used.
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //Use the value of "sprintSpeed" if left-shift is held down, otherwise use the value of "moveSpeed";
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        //Update the GameObject's position with the detected move direction and speed.
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}