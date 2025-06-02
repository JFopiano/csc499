using UnityEngine;
using System.Collections.Generic;

public class grab : MonoBehaviour
{
    public GameObject Object;
    public Transform PlayerTransform;
    public float range = 3f;
    public float Go = 100f;
    public Camera Camera;
    public float rotationSpeed = 100f;     // Speed at which objects rotate
    public float scaleSpeed = 0.5f;        // Speed at which objects scale up/down

    public float holdDistance = 10f;  // Fixed hold distance (distance at which the object is held from your camera)

    public IrisVisualizer irisVisualizer; // Reference to the IrisVisualizer

    public enum RotationAxis { Right, Up, Forward }
    public RotationAxis rollAxis = RotationAxis.Right;

    
    private List<GameObject> leftSideObjects = new List<GameObject>();
    private List<GameObject> rightSideObjects = new List<GameObject>();

    // Private variables to track state
    private bool isHoldingObject = false;  // Tracks whether player is currently holding an object
    private Vector3 originalScale;         // Stores the initial scale of the object for reset purposes
    

    void Start() {
        // Store the original scale when the script starts
        if (Object != null) {
            originalScale = Object.transform.localScale;
        }
    }

    void Update() {
        // F key toggles between picking up and dropping objects
        if (Input.GetKeyDown(KeyCode.F)) {
            if (isHoldingObject) {
                Drop();                    // Drop if holding
            } else {
                StartPickUp();            // Try to pick up if not holding
            }
        }

        // Only allow manipulation of object if holding an object
        if (isHoldingObject) {
            // Rotate object when Q or E is pressed
            if (Input.GetKey(KeyCode.Q))  // Changed from GetKeyDown to GetKey for smoother rotation
            {
                // Rotate around Y axis (up) in world space
                Object.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                // Rotate around Y axis (up) in world space
                Object.transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
            }
            // Z-axis rotation (roll)
            if (Input.GetKey("2"))
            {
                Object.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey("1"))
            {
                Object.transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
            }

            // Control the scale of the object mouse scroll wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0) { // If you're scrolling, change the scale
                ChangeScale(scroll);
            }

            // After any manipulation, update classification
            UpdateClassification();
            float score = CalculateScore();
            //Debug.Log($"Current classification score: {score}");
        }
    }

    void StartPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                PickUp();
            }
        }
    }

    void PickUp()
{
    Rigidbody rb = Object.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true;
        Object.transform.SetParent(PlayerTransform);
        isHoldingObject = true;

        

        Debug.Log(Camera.transform.position + Camera.transform.forward * holdDistance);
        
        Vector3 holdPosition = Camera.transform.position + Camera.transform.forward * 15f;
        // Set initial position at fixed distance from camera
        Object.transform.position = holdPosition;

        // Set the rotation of the object pecific rotation when picked up
        Object.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}

    // Handles dropping the held object
    void Drop()
    {
        Rigidbody rb = Object.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Object.transform.SetParent(null);
            //PlayerTransform.DetachChildren();
            //rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isHoldingObject = false;
        }
    }

    
    // Handles general rotation around any axis
    void RotateObject(Vector3 axis)
    {
        Object.transform.Rotate(axis * rotationSpeed * Time.deltaTime, Space.World);
    }

    // Handles rolling around a specific axis
    void RollObject(int direction)
    {
        Vector3 axis;
        switch (rollAxis)
        {
            case RotationAxis.Right:
                axis = Object.transform.right;
                break;
            case RotationAxis.Up:
                axis = Object.transform.up;
                break;
            case RotationAxis.Forward:
                axis = Object.transform.forward;
                break;
            default:
                axis = Object.transform.right;
                break;
        }
        Object.transform.Rotate(axis * direction, rotationSpeed * Time.deltaTime, Space.Self);
    }

    // Handles scaling (width) of the object
    void ChangeScale(float mouseScrollWheelInputAxis) {
        Debug.Log("Attempting to change scale");
        
        // Calculate new scale based on scroll input, preserving Y and Z
        Vector3 newScale = Object.transform.localScale;
        newScale.x += mouseScrollWheelInputAxis * scaleSpeed;
        
        // Only apply new scale if within acceptable limits
        float minScale = 0.1f; // Set to match the default scale from Transform component
        float maxScale = 8f;
        
        // Clamp x component between min and max, preserve y and z
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        
        // Apply the new scale
        Object.transform.localScale = newScale;
        
        Debug.Log($"Scale changed to: {newScale}");
    }

    void UpdateClassification()
    {
        if(irisVisualizer == null) return;

        leftSideObjects.Clear();
        rightSideObjects.Clear();

        Plane separationPlane = new Plane(Object.transform.up, Object.transform.position);
        

        foreach (var dataPoint in irisVisualizer.dataPoints)
        {
            if (separationPlane.GetSide(dataPoint.transform.position))
            {
                rightSideObjects.Add(dataPoint);
            }
            else
            {
                leftSideObjects.Add(dataPoint);
            }
        }

        Debug.Log($"Left side: {leftSideObjects.Count}, Right side: {rightSideObjects.Count}");
    }

    float CalculateScore()
    {
        if(irisVisualizer == null) return 0f;

        int correctClassifications = 0;
        string leftSideSpecies = GetMajoritySpecies(leftSideObjects);
        string rightSideSpecies = GetMajoritySpecies(rightSideObjects);

        foreach (var obj in leftSideObjects)
        {
            if (obj.GetComponent<Renderer>().material.color == GetColorForSpecies(leftSideSpecies))
                correctClassifications++;
        }

        foreach (var obj in rightSideObjects)
        {
            if (obj.GetComponent<Renderer>().material.color == GetColorForSpecies(rightSideSpecies))
                correctClassifications++;
        }

        return (float)correctClassifications / irisVisualizer.dataPoints.Count;
    }

    string GetMajoritySpecies(List<GameObject> objects)
    {
        int setosa = 0, versicolor = 0, virginica = 0;
        foreach (var obj in objects)
        {
            Color color = obj.GetComponent<Renderer>().material.color;
            if (color == Color.red) setosa++;
            else if (color == Color.green) versicolor++;
            else if (color == Color.blue) virginica++;
        }
        return (setosa > versicolor && setosa > virginica) ? "setosa" :
               (versicolor > virginica) ? "versicolor" : "virginica";
    }

    Color GetColorForSpecies(string species)
    {
        switch (species.ToLower())
        {
            case "setosa": return Color.red;
            case "versicolor": return Color.green;
            case "virginica": return Color.blue;
            default: return Color.white;
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Pickup"))
    //     {
    //         Debug.Log("Object entered trigger zone: " + other.gameObject.name);
    //         Object = other.gameObject;
    //         StartPickUp();
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Pickup"))
    //     {
    //         Debug.Log("Object exited trigger zone: " + other.gameObject.name);
    //         Drop();
    //     }
    // }
}

/*
Object Interaction:

Pick up objects manually using the 'F' key
Drop objects manually using the 'F' key
Automatic pickup when entering a trigger zone of a "Pickup" tagged object
Automatic drop when exiting a trigger zone of a "Pickup" tagged object


Object Manipulation:

Rotate the held object around world axes (Q/E for Y-axis, R/V for X-axis, T/G for Z-axis)
Roll the held object around its local axis (Z/X keys)
Scale the width of the held object (1/2 keys)


Iris Dataset Interaction:

Classify Iris data points based on the position of the held object (separation plane)
Update classification whenever the object is manipulated


Scoring:

Calculate a score based on the current classification of Iris data points
Determine the majority species on each side of the separation plane
Count correct classifications based on the majority species


Visualization:

Separate data points into left and right side lists based on the separation plane
Log the number of data points on each side of the plane


Utility Functions:

Get the majority species from a list of objects
Get the color associated with each Iris species


Physics Interaction:

Use raycasting to detect objects for manual pickup
Manage Rigidbody components when picking up and dropping objects


Debugging:

Log various actions and states (pickup, drop, hit detection, classification results)


Customization:

Allow selection of rotation axis for rolling (Right, Up, Forward)
Adjustable parameters for range, rotation speed, and scale change speed



This script serves as the main interaction controller for your educational game, combining object manipulation with data visualization and classification tasks centered around the Iris dataset.
*/