using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;   
using UnityEngine.XR;

public class PlaneDeformer : MonoBehaviour {
    private Mesh mesh;
    private List<Vector3> vertices;
    public GameObject dotPrefab; // Reference to the dot prefab
    public List<GameObject> dots; // List to store the instantiated dots
    public List<int> dotIndices; // List to store the indices of the vertices that have dots

    public GameObject dotPrefabR; // Reference to the red dot prefab
    public List<GameObject> dotsR; // List to store the red instantiated dots
    public List<int> dotIndicesR; // List to store the indices of the vertices that have red dots

    // Reference to the XR controller's ray interactor
    public XRRayInteractor rayInteractor;
    
    void Start() {
        // Get the mesh from the MeshFilter component on the same GameObject
        Debug.Log("starting");
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new List<Vector3>(mesh.vertices);

        // Ensure the plane has a collider
        if (GetComponent<Collider>() == null)
        {
            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.convex = true; // Set to convex if needed
        }

        // Initialize lists
        dots = new List<GameObject>();
        dotIndices = new List<int>();
        dotsR = new List<GameObject>();
        dotIndicesR = new List<int>();

        // Find the ray interactor if not assigned
        if (rayInteractor == null)
        {
            // Try to find the ray interactor in the scene
            XRRayInteractor[] interactors = FindObjectsOfType<XRRayInteractor>();
            foreach (var interactor in interactors)
            {
                // Find the right-hand controller ray interactor
                if (interactor.gameObject.name.Contains("Ray Interactor") && 
                    interactor.transform.parent.name.Contains("Right"))
                {
                    rayInteractor = interactor;
                    Debug.Log("Found Ray Interactor: " + rayInteractor.gameObject.name);
                    break;
                }
            }
        }

        // Instantiate dots at the vertices positions
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            float radius = 2.0f;

            if (vertex.magnitude > (radius+1)) {
                GameObject dotR = Instantiate(dotPrefabR, transform.TransformPoint(vertex), Quaternion.identity, transform);
                dotsR.Add(dotR);
                dotIndicesR.Add(i);

                // Add interaction component and setup material
                Renderer renderer = dotR.GetComponent<Renderer>();
                if (renderer != null) {
                    Material material = new Material(Shader.Find("Standard"));
                    material.EnableKeyword("_EMISSION");
                    material.color = Color.red;
                    material.SetColor("_EmissionColor", Color.black);
                    renderer.material = material;

                    DotInteraction interaction = dotR.AddComponent<DotInteraction>();
                    interaction.Initialize(material, Color.red);
                }

                // Ensure there's a trigger collider
                SphereCollider collider = dotR.GetComponent<SphereCollider>();
                if (collider == null) {
                    collider = dotR.AddComponent<SphereCollider>();
                }
                collider.isTrigger = true;
            }
            else if(vertex.magnitude <= radius) {
                GameObject dot = Instantiate(dotPrefab, transform.TransformPoint(vertex), Quaternion.identity, transform);
                dots.Add(dot);
                dotIndices.Add(i);

                // Add interaction component and setup material
                Renderer renderer = dot.GetComponent<Renderer>();
                if (renderer != null) {
                    Material material = new Material(Shader.Find("Standard"));
                    material.EnableKeyword("_EMISSION");
                    material.color = Color.green;
                    material.SetColor("_EmissionColor", Color.black);
                    renderer.material = material;

                    DotInteraction interaction = dot.AddComponent<DotInteraction>();
                    interaction.Initialize(material, Color.green);
                }

                // Ensure there's a trigger collider
                SphereCollider collider = dot.GetComponent<SphereCollider>();
                if (collider == null) {
                    collider = dot.AddComponent<SphereCollider>();
                }
                collider.isTrigger = true;
            }
        }

        // Update the mesh with the new vertices positions
        mesh.vertices = vertices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh; // Update the collider
    }

    // Existing method to deform the plane when clicking
    void Update() {
        bool triggerPressed = CheckTriggerButtonPressed();
        if(triggerPressed) {
            // The XR Ray Interactor already has visual line rendering
            // Get the ray from the interactor
            Ray controllerRay = new Ray(rayInteractor.rayOriginTransform.position, 
                                            rayInteractor.rayOriginTransform.forward);
            // Cast the ray and check for hits
            RaycastHit hit;
            if (Physics.Raycast(controllerRay, out hit)) {
                //Debug.Log("Controller ray hit at: " + hit.point);
                Vector3 pressurePoint = transform.InverseTransformPoint(hit.point);
                //Debug.Log("Pressure point in local space: " + pressurePoint);
                ApplyPressure(pressurePoint, 0.2f, 1.6f); // Apply gentle pressure with each click
            }
            else {
                //Debug.Log("Controller ray did not hit any collider");
            }
        }
    }

    // Check if the primary button is pressed on the right controller
    private bool CheckPrimaryButtonPressed() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, devices);
        
        foreach (var device in devices) {
            bool primaryButtonPressed = false;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed) {
                return true;
            }
        }
        return false;
    }
    
    // Check if the secondary button is pressed on the right controller
    private bool CheckSecondaryButtonPressed() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, devices);
        
        foreach (var device in devices) {
            bool secondaryButtonPressed = false;
            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonPressed) && secondaryButtonPressed) {
                return true;
            }
        }
        return false;
    }

    private bool CheckTriggerButtonPressed() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, devices);
        
        foreach (var device in devices) {
            bool triggerButtonPressed = false;  
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonPressed) && triggerButtonPressed) {
                return true;
            }
        }
        return false;
    }
    

    public void ApplyPressure(Vector3 pressurePoint, float pressureAmount, float falloff)
    {
        //Debug.Log("Applying pressure at: " + pressurePoint);
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            //Debug.Log("Original vertex position: " + vertex);
            float distance = Vector3.Distance(vertex, pressurePoint);

            // Adjust the vertex.y based on the distance from the pressure point using a Gaussian function
            float deformation = pressureAmount * Mathf.Exp(-Mathf.Pow(distance, 2) / (2 * Mathf.Pow(falloff, 2)));
            vertex.y -= deformation; // Subtracting to deform the other way

            vertices[i] = vertex;
            //Debug.Log("Updated vertex position: " + vertex);
        }

        // Update the positions of the dots
        for (int i = 0; i < dots.Count; i++)
        {
            int vertexIndex = dotIndices[i];
                        Vector3 vertex = vertices[vertexIndex];
            dots[i].transform.position = transform.TransformPoint(vertex);
        }

        // Update the positions of the red dots
        for (int i = 0; i < dotsR.Count; i++)
        {
            int vertexIndexR = dotIndicesR[i];
            Vector3 vertexR = vertices[vertexIndexR];
            dotsR[i].transform.position = transform.TransformPoint(vertexR);
            //Debug.Log("Updated red dot position: " + dotsR[i].transform.position);
        }

        mesh.vertices = vertices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds(); // Recalculate bounds
        GetComponent<MeshCollider>().sharedMesh = mesh; // Update the collider
    }



    // Method to apply deformation at the specified decision point
    public void ApplyAutomaticPressure(Vector3 decisionPoint, float pressureAmount, float falloff)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            //Debug.Log("Original vertex position: " + vertex);
            float distance = Vector3.Distance(vertex, decisionPoint);

            // Adjust the vertex.y based on the distance from the decision point using a Gaussian function
            float deformation = pressureAmount * Mathf.Exp(-Mathf.Pow(distance, 2) / (2 * Mathf.Pow(falloff, 2)));
            vertex.y -= deformation; // Subtracting to deform the other way

            vertices[i] = vertex;
            //Debug.Log("Updated vertex position: " + vertex);
        }

        // Update the positions of the dots
        for (int i = 0; i < dots.Count; i++)
        {
            int vertexIndex = dotIndices[i];
            Vector3 vertex = vertices[vertexIndex];
            dots[i].transform.position = transform.TransformPoint(vertex);
        }

        // Update the positions of the red dots
        for (int i = 0; i < dotsR.Count; i++)
        {
            int vertexIndexR = dotIndicesR[i];
            Vector3 vertexR = vertices[vertexIndexR];
            dotsR[i].transform.position = transform.TransformPoint(vertexR);
            //Debug.Log("Updated red dot position: " + dotsR[i].transform.position);
        }

        mesh.vertices = vertices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds(); // Recalculate bounds
        GetComponent<MeshCollider>().sharedMesh = mesh; // Update the collider
    }

    public List<Vector3> GetVertices()
    {
        return new List<Vector3>(mesh.vertices);
    }

    public void UpdateVertices(List<Vector3> updatedVertices)
    {
        mesh.vertices = updatedVertices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh; // Update collider

        // Update dot positions
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].transform.position = transform.TransformPoint(updatedVertices[dotIndices[i]]);
        }

        for (int i = 0; i < dotsR.Count; i++)
        {
            dotsR[i].transform.position = transform.TransformPoint(updatedVertices[dotIndicesR[i]]);
        }
    }
}

public class DotInteraction : MonoBehaviour {
    private Material material;
    private Color baseColor;
    private bool isGlowing = false;
    private float glowIntensity = 2.0f;
    
    public void Initialize(Material mat, Color color) {
        material = mat;
        baseColor = color;
    }
    
    void OnTriggerEnter(Collider other) {
        if (!isGlowing) {
            StartGlow();
        }
    }
    
    void OnTriggerExit(Collider other) {
        if (isGlowing) {
            StopGlow();
        }
    }
    
    private void StartGlow() {
        isGlowing = true;
        material.SetColor("_EmissionColor", baseColor * glowIntensity);
    }
    
    private void StopGlow() {
        isGlowing = false;
        material.SetColor("_EmissionColor", Color.black);
    }
}
