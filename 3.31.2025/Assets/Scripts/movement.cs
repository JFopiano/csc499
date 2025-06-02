using UnityEngine;
using UnityEngine.EventSystems; // Required for UI interaction
using UnityEngine.SceneManagement; // Required for scene changes

public class Movement : MonoBehaviour
{
    [SerializeField] Transform MainCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    float velocityY;
    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    bool isCursorLocked;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockCursor(cursorLock); // Initial cursor lock
    }

    void Update()
    {
        // Unlock cursor when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false);
        }

        // Only lock the cursor if not over UI
        if (!IsCursorOverUI() && Input.GetMouseButtonDown(0) && !isCursorLocked)
        {
            LockCursor(true);
        }

        // Only allow movement when the cursor is locked
        if (isCursorLocked)
        {
            UpdateMouse();
            UpdateMove();
        }
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        MainCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }

    // Function to lock or unlock the cursor
    void LockCursor(bool lockCursor)
    {
        isCursorLocked = lockCursor;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    // Check if the cursor is over a UI element
    bool IsCursorOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    // Public method to change scenes (attach to UI button)
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
