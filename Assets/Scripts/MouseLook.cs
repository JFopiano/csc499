using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; //Sets sensitivity for mouse
    public Transform playerBody; //References the Player

    float xRotation = 0f; //Up and Down Rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock and hide cursor to the center of the screen
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; //Gets how much the mouse has moved in X axis
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; //Gets how much the mouse has moved in Y axis

        xRotation -= mouseY; //Up and down movement
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // prevent over-rotation when looking up and down

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Moves the camera up and down
        playerBody.Rotate(Vector3.up * mouseX); //Rotates entire body when looking left and right
    }
}

