using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; //Handles collision and movement
    public float speed = 12f; //Speed of the player

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); //Left and Right
        float z = Input.GetAxis("Vertical");  //Front and Back

        Vector3 move = transform.right * x + transform.forward * z; //Makes the player move in base of where they are looking
        controller.Move(move * speed * Time.deltaTime); //
    }
}

