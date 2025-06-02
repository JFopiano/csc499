using UnityEngine;
using UnityEngine.UI;

public class SimulatePointClick : MonoBehaviour
{
    public Button simulateClickButton; // Assign your UI button in the inspector
    public Vector3 targetPoint = new Vector3(-124.95f, 1.92f, -33.29f); // The point to simulate the click to start from
    public Vector3 clickDirection = Vector3.left; // Direction of the simulated click
    public float maxDistance = 100f; // Maximum distance for the ray
    public LayerMask clickableLayer; // Layer to detect clickable objects
    public GameObject planeGenObject; // Assign the PlaneGen GameObject in the inspector

    //Is changing the target point somewhere idk how

    void Start()
    {
        // Attach the SimulateClick method to the button click event
        simulateClickButton.onClick.AddListener(SimulateClick);
    }

    void SimulateClick()
    {
        
       Debug.Log("Target Point check 1 " + targetPoint);
    // Normalize the direction
        Vector3 direction = clickDirection.normalized;

        // Create a ray from the target point in the specified direction
        Ray ray = new Ray(targetPoint, direction);

        Debug.Log("Target Point check 2 " + targetPoint);

        RaycastHit hit;

        // Try raycasting first
        if (Physics.Raycast(ray, out hit, maxDistance, clickableLayer))
        {
            // If the ray hits an object, trigger the interaction
            Debug.Log("Simulated click at: " + hit.point + " on object: " + hit.collider.gameObject.name+ " " + targetPoint);

            // Send an interaction message to the hit object
            hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("No object hit. Trying direct trigger...");

            // Fallback to direct trigger
            DirectTrigger();
        }

        // Visualize the ray in the Scene view
        Debug.DrawRay(targetPoint, direction * maxDistance, Color.red, 2f);
    }

    void DirectTrigger()
    {
        // Check if PlaneGen object is assigned
        if (planeGenObject != null)
        {
            // Simulate a direct interaction
            planeGenObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            Debug.Log("Direct trigger on object: " + planeGenObject.name);
        }
        else
        {
            Debug.LogError("PlaneGen object not assigned in the inspector.");
        }
    }
}
