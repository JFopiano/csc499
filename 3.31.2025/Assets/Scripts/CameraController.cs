using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    private float rotationX = 0f;
    private bool isCameraControlEnabled = true;

    void Update()
    {
        // Toggle camera control with the 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCameraControlEnabled = !isCameraControlEnabled;
            Cursor.visible = !isCameraControlEnabled;
            Cursor.lockState = isCameraControlEnabled ? CursorLockMode.Locked : CursorLockMode.None;
        }

        // Only rotate camera if control is enabled and not over UI
        if (isCameraControlEnabled && !EventSystem.current.IsPointerOverGameObject())
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}